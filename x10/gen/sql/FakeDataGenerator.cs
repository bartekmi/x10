using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using x10.model.definition;
using x10.model.metadata;
using x10.utils;
using x10.ui.composition;
using x10.parsing;
using x10.gen.sql.primitives;
using x10.gen.sql.parser;

namespace x10.gen.sql {

  #region Helper Clases

  public class EntityInfo {
    public Entity Entity;
    public List<Row> Rows = new List<Row>();

    internal int NextId = 1;
    internal DataGenerationContext Context;

    // Derived
    public IEnumerable<Row> NonOwnedRows => Rows.Where(x => !x.IsOwned);

    internal int? RandomExistingId(Random random, Association association) {
      if (Rows.Count == 0)
        return null;
      return random.Next(1, NextId);
    }
  }
  #endregion

  public class FakeDataGenerator {

    public int? TestingReducedNumberOfRows { get; set; }
    public bool AllowMultipleReverseAssociationsToSameEntity { get; set; }

    #region Members, Top Level, Constructor, Initialization
    public Dictionary<Entity, EntityInfo> EntityInfos;


    private static readonly SqlRange DEFAULT_ASSOCIATION_RANGE = SqlRange.Parse("0..3");

    private readonly MessageBucket _messages;
    private readonly Random _random;
    private readonly AtomicDataGenerator _atomicDataGenerator;
    private readonly DataGenLanguageParser _parser;
    private readonly string _dataFilesRoot;
    private readonly IEnumerable<Entity> _entities;
    private DeclaredColumnsCalculator _declaredColumnCalculator;
    private List<Entity> _sortedEntities;
    private HashSet<Entity> _unprocessedRootEntities;

    public FakeDataGenerator(MessageBucket messages, IEnumerable<Entity> entities, Random random, string dataFilesRoot) {
      _messages = messages;
      _random = random;
      _entities = entities;
      _dataFilesRoot = dataFilesRoot;
      _atomicDataGenerator = new AtomicDataGenerator(messages);
      _parser = new DataGenLanguageParser(messages);
    }

    private void Initialize() {
      _declaredColumnCalculator = new DeclaredColumnsCalculator(_entities, AllowMultipleReverseAssociationsToSameEntity);
      IEnumerable<Entity> entities = _declaredColumnCalculator.GetRealEntities();

      IEnumerable<Entity> rootLevelEntities = entities.Where(x => GetCount(x) > 0);

      EntityInfos = entities.ToDictionary(x => x, (x) => new EntityInfo() {
        Context = DataGenerationContext.CreateContext(_messages, _parser, _random, x, _dataFilesRoot),
        Entity = x,
      });

      _unprocessedRootEntities = new HashSet<Entity>(rootLevelEntities);
      _sortedEntities = GetSortedEntities(entities);  // Will blow up if circular ref's found
    }

    private List<Entity> GetSortedEntities(IEnumerable<Entity> realEntities) {
      IEnumerable<Edge<Entity>> edges =
        _declaredColumnCalculator.AllForward.Select(x => new Edge<Entity>(x.ActualOwner, x.Association.ReferencedEntity))
        .Concat(_declaredColumnCalculator.AllReverse.Select(x => new Edge<Entity>(x.Association.ReferencedEntity, x.ActualOwner)));

      List<Entity> sorted = GraphUtils.SortDirectAcyclicGraph(realEntities, edges);
      sorted.Reverse();   // Reverse so as to generate starting from no FK dependencies
      return sorted;
    }
    #endregion

    #region SQL-Specific
    public void GenerateSql(string filename) {
      using (TextWriter writer = new StreamWriter(filename))
        GenerateSqlPrivate(writer);
    }

    public string GenerateSqlIntoString() {
      using (TextWriter writer = new StringWriter()) {
        GenerateSqlPrivate(writer);
        return writer.ToString();
      }
    }

    public void GenerateSqlPrivate(TextWriter writer) {
      GenerateData();
      DumpSql(writer);
    }

    #region Write to File

    // Sample:

    // INSERT INTO my_table (id, col1, col2, ...) VALUES
    // (1, val1, val2, ...),
    // ...
    // (N, val1, val2, ...);
    private void DumpSql(TextWriter writer) {
      IEnumerable<EntityInfo> infos = _sortedEntities
        .Select(x => EntityInfos[x])
        .Where(x => x.Rows.Count > 0);

      foreach (EntityInfo entityInfo in infos) {
        // Write the INSERT statement
        Entity entity = entityInfo.Entity;
        IEnumerable<string> dbColumnNames = _declaredColumnCalculator.GetDeclaredColumns(entity)
          .Select(x => SqlSchemaGenerator.GetDbColumnName(x));

        writer.WriteLine("INSERT INTO \"{0}\" (\"id\", {1}) VALUES",
          SqlSchemaGenerator.GetTableName(entity),
          string.Join(", ", dbColumnNames.Select(x => string.Format("\"{0}\"", x)))
        );

        // Write the data
        foreach (Row row in entityInfo.Rows) {
          writer.WriteLine("({0}, {1}){2}",
            row.Id,
            string.Join(", ", row.Values.Select(x => ToSql(x))),
            row == entityInfo.Rows.Last() ? ";" : ",");
        }

        writer.WriteLine();
      }
    }

    private static string ToSql(MemberAndValue memberAndValue) {
      Member member = memberAndValue.Member;
      object value = memberAndValue.Value;

      if (value == null)
        return "NULL";

      bool quote = false;

      if (member is X10Attribute x10Attribute) {
        DataType dataType = x10Attribute.DataType;

        if (dataType == DataTypes.Singleton.Date ||
        dataType == DataTypes.Singleton.String ||
        dataType == DataTypes.Singleton.Timestamp ||
        dataType is DataTypeEnum)
          quote = true;
      }

      if (quote)
        return string.Format("'{0}'", value.ToString().Replace("'", "''"));
      else
        return value.ToString();
    }
    #endregion
    #endregion

    #region Data Creation

    public void GenerateData() {
      Initialize();
      while (_unprocessedRootEntities.Count > 0) {          // Until no more root-level entities
        Entity entity = _unprocessedRootEntities.First();
        GenerateForEntity(entity);
      }
    }

    private void GenerateForEntity(Entity entity) {
      if (!_unprocessedRootEntities.Contains(entity))
        return;
      _unprocessedRootEntities.Remove(entity);  // Done with this one!

      // Generate all rows
      int count = GetCount(entity);
      for (int ii = 0; ii < count; ii++)
        GenerateRow(entity, null, null);
    }

    private void GenerateRow(Entity entity, Row parentRow, Association parentAssociation) {
      Row row = GenerateSingleRow(entity, parentRow, parentAssociation);

      // Generate rows for owned associations
      foreach (Association association in entity.Associations.Where(x => x.Owns)) {
        int count = GetCount(association);
        for (int ii = 0; ii < count; ii++)
          GenerateRow(association.ReferencedEntity, row, association);
      }
    }

    private Row GenerateSingleRow(Entity entity, Row parentRow, Association parentAssociation) {
      Row row = CreateAndStoreRow(entity, parentAssociation);

      MemberAndOwner parentAssociationMO = null;
      if (parentRow != null) {
        IEnumerable<MemberAndOwner> reverses = _declaredColumnCalculator.GetReverseAssociations(entity);

        if (AllowMultipleReverseAssociationsToSameEntity)
          parentRow.AddChildAssociation(parentAssociation, row);
        else {
          IEnumerable<MemberAndOwner> parentAssociations = reverses.Where(x => x.ActualOwner == parentRow.Entity);

          // This is a serious limitation in my implementation. It does not handle the simple case where an entity has two addresses,
          // e.g. a physical address and a mailing address, etc. Not viable for a real system, but has significant implications
          // for how I structured the SQL generation.
          if (parentAssociations.Count() != 1)
            throw new Exception(string.Format("Entity {0} referenced by Entity {1} must have exactly one association of that type, but has {2}",
              entity.Name, parentRow.Entity.Name, parentAssociations.Count()));
          parentAssociationMO = parentAssociations.Single();
        }
      }

      DataGenerationContext context = EntityInfos[entity].Context;
      DataFileRow externalRow = context.GetRandomExternalFileRow();
      IEnumerable<MemberAndOwner> allColumns = _declaredColumnCalculator.GetDeclaredColumns(entity);

      foreach (MemberAndOwner memberAndOwner in allColumns)
        if (AllowMultipleReverseAssociationsToSameEntity) {
          X10Attribute attribute = memberAndOwner.Attribute;
          Association assoc = memberAndOwner.Association;
          if (attribute != null)
            row.Values.Add(_atomicDataGenerator.Generate(_random, context, attribute, externalRow));
          else if (assoc?.Owns == true) {
            // Do nothing
          } else if (assoc?.Owns == false) {
            row.Values.Add(CreateNonOwnedAssociationValue(assoc));
          } else
            throw new Exception("Member is unexpected: " + memberAndOwner.Member);
        } else {
          switch (memberAndOwner.Type) {
            case ColumnType.Attribute:
              row.Values.Add(_atomicDataGenerator.Generate(_random, context, memberAndOwner.Attribute, externalRow));
              break;
            case ColumnType.ForwardAssociation:
              row.Values.Add(CreateNonOwnedAssociationValue(memberAndOwner.Association));
              break;
            case ColumnType.ReverseAssociation:
              row.Values.Add(new MemberAndValue() {
                Member = memberAndOwner.Association,
                Value = memberAndOwner == parentAssociationMO ? parentRow.Id : (int?)null,
              });
              break;
            default:
              throw new Exception("Unexpected member and association type: " + memberAndOwner.Type);
          }
        }

      return row;
    }

    private MemberAndValue CreateNonOwnedAssociationValue(Association association) {
      Entity entity = association.ReferencedEntity;
      GenerateForEntity(entity);

      int? value = null;

      if (ShouldCreateBasedOnProbability(association)) {
        value = EntityInfos[entity].RandomExistingId(_random, association);
        if (association.IsMandatory && value == null)
          _messages.AddError(association.TreeElement, "Association {0} is mandatory, but Entity {1} does not specify '{2}' attribute for data generation",
            association, entity.Name, DataGenLibrary.QUANTITY);
      }

      return new MemberAndValue() {
        Member = association,
        Value = value,
      };
    }
    #endregion

    #region Utilities

    private Row CreateAndStoreRow(Entity entity, Association parentAssociation) {
      EntityInfo info = EntityInfos[entity];
      Row row = new Row() {
        Entity = entity,
        Id = info.NextId++,
        OwnedByAssociation = parentAssociation,
      };
      info.Rows.Add(row);
      return row;
    }

    private int GetCount(Association association) {
      if (association.IsMany) {
        SqlRange range = association.FindValue<SqlRange>(DataGenLibrary.QUANTITY) ?? DEFAULT_ASSOCIATION_RANGE;
        return range.GetRandom(_random);
      } else
        return ShouldCreateBasedOnProbability(association) ? 1 : 0;
    }

    private bool ShouldCreateBasedOnProbability(Member member) {
      if (member.IsMandatory)
        return true;

      object probabilityObj = member.FindValue(DataGenLibrary.PROBABILITY);
      if (probabilityObj == null)
        return true;  // Default is to create

      double probability = (double)probabilityObj;
      return _random.NextDouble() < probability;
    }

    private int GetCount(Entity entity) {
      if (!entity.FindValue<int>(DataGenLibrary.QUANTITY, out int quantity)) {
        DataGenerationContext context = EntityInfos[entity].Context;
        if (context.ExternalDataFiles.Count == 1)
          return context.ExternalDataFiles.Single().Count;
        return 0;
      }

      return TestingReducedNumberOfRows ?? quantity;
    }
    #endregion
  }
}

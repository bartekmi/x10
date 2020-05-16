using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using x10.gen.sql.primitives;
using x10.model;
using x10.model.definition;
using x10.model.metadata;
using x10.utils;
using x10.ui.composition;
using x10.parsing;

namespace x10.gen.sql {
  public class FakeDataGenerator {

    private static readonly int? TEST_REDUCED_NUMBER_OF_ROWS = null;

    #region Helper Clases
    private class Row {
      internal int Id;
      internal Entity Entity;
      internal List<MemberAndValue> Values = new List<MemberAndValue>();

      public object[] ValueForSql { get; internal set; }
    }

    private class EntityInfo {
      internal int NextId = 1;
      internal List<Row> Rows = new List<Row>();
      internal DataGenerationContext Context;
      internal Entity Entity;
      internal int? OrderIndex;

      internal int? RandomExistingId(Random random, Association association) {
        if (Rows.Count == 0)
          return null;
        return random.Next(1, NextId);
      }
    }
    #endregion

    #region Private Variables, Top Level, Constructor
    private static readonly SqlRange DEFAULT_ASSOCIATION_RANGE = SqlRange.Parse("0..3");

    private readonly MessageBucket _messages;
    private readonly Random _random;
    private readonly Dictionary<Entity, EntityInfo> _entityInfo;
    private readonly HashSet<Entity> _unprocessedRootEntities;
    private readonly AtomicDataGenerator _atomicDataGenerator;
    private readonly DataGenLanguageParser _parser;
    private readonly DeclaredColumnsCalculator _declaredColumnCalculator;

    private int _tableOrderIndex;

    public FakeDataGenerator(MessageBucket messages, IEnumerable<Entity> entities, Random random) {
      _messages = messages;
      _random = random;
      _atomicDataGenerator = new AtomicDataGenerator(messages);
      _parser = new DataGenLanguageParser(messages);
      _declaredColumnCalculator = new DeclaredColumnsCalculator(entities);

      entities = _declaredColumnCalculator.GetRealEntities();

      IEnumerable<Entity> rootLevelEntities = entities.Where(x => GetCount(x) > 0);
      _unprocessedRootEntities = new HashSet<Entity>(rootLevelEntities);

      _entityInfo = entities.ToDictionary(x => x, (x) => new EntityInfo() {
        Context = DataGenerationContext.CreateContext(_messages, _parser, _random, x),
        Entity = x,
      });
    }

    public void Generate(string filename) {
      using (TextWriter writer = new StreamWriter(filename))
        GeneratePrivate(writer);
    }

    public string GenerateIntoString() {
      using (TextWriter writer = new StringWriter()) {
        GeneratePrivate(writer);
        return writer.ToString();
      }
    }

    public void GeneratePrivate(TextWriter writer) {
      Generate();
      DumpData(writer);
    }
    #endregion

    #region Write to File

    // Sample:

    // INSERT INTO my_table (id, col1, col2, ...) VALUES
    // (1, val1, val2, ...),
    // ...
    // (N, val1, val2, ...);
    private void DumpData(TextWriter writer) {
      IEnumerable<EntityInfo> infos = _entityInfo.Values
        .Where(x => x.Rows.Count > 0)
        .OrderBy(x => x.OrderIndex);

      foreach (EntityInfo entityInfo in infos) {
        // Write the INSERT statement
        Entity entity = entityInfo.Entity;
        IEnumerable<string> dbColumnNames = _declaredColumnCalculator.GetDeclaredColumns(entity)
          .Select(x => SqlSchemaGenerator.GetDbColumnName(x));

        writer.WriteLine("INSERT INTO {0} (id, {1}) VALUES",
          SqlSchemaGenerator.GetTableName(entity),
          string.Join(", ", dbColumnNames)
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
        return string.Format("'{0}'", value);
      else
        return value.ToString();
    }
    #endregion

    #region Data Creation

    private void Generate() {
      while (_unprocessedRootEntities.Count > 0) {          // Until no more root-level entities
        Entity entity = _unprocessedRootEntities.First();
        GenerateForEntity(entity);
      }
    }

    private void GenerateForEntity(Entity entity) {
      // Generate all rows
      int count = GetCount(entity);
      for (int ii = 0; ii < count; ii++)
        GenerateRow(entity, null);

      _unprocessedRootEntities.Remove(entity);  // Done with this one!
    }

    private void GenerateRow(Entity entity, Row parent) {
      Row row = GenerateSingleRow(entity, parent);

      // Generate rows for owned associations
      foreach (Association association in entity.Associations.Where(x => x.Owns)) {
        int count = GetCount(association);
        for (int ii = 0; ii < count; ii++)
          GenerateRow(association.ReferencedEntity, row);
      }
    }

    private Row GenerateSingleRow(Entity entity, Row parent) {
      Row row = CreateAndStoreRow(entity);

      MemberAndOwner linkToParent = null;
      if (parent != null) {
        IEnumerable<MemberAndOwner> reverses = _declaredColumnCalculator.GetReverseAssociations(entity);
        IEnumerable<MemberAndOwner> parentAssociations = reverses.Where(x => x.ActualOwner == parent.Entity);

        if (parentAssociations.Count() != 1)
          throw new Exception(string.Format("Entity {0} referenced by Entity {1} must have exactly one association of that type, but has {2}",
            entity.Name, parent.Entity.Name, parentAssociations.Count()));
        linkToParent = parentAssociations.Single();
      }

      DataGenerationContext context = _entityInfo[entity].Context;
      DataFileRow externalRow = context.GetRandomExternalFileRow();
      IEnumerable<MemberAndOwner> allColumns = _declaredColumnCalculator.GetDeclaredColumns(entity);

      foreach (MemberAndOwner member in allColumns)
        switch (member.Type) {
          case ColumnType.Attribute:
            row.Values.Add(_atomicDataGenerator.Generate(_random, context, member.Attribute, externalRow));
            break;
          case ColumnType.ForwardAssociation:
            row.Values.Add(CreateForwardAssociationValue(member.Association));
            break;
          case ColumnType.ReverseAssociation:
              row.Values.Add(new MemberAndValue() {
                Member = member.Association,
                Value = member == linkToParent ? parent.Id : (int?)null,
              });
            break;
          default:
            throw new Exception("Unexpected member and associatoin type: " + member.Type);
        }

      SetEntityOrderIfFirstTime(entity);
      return row;
    }

    private MemberAndValue CreateForwardAssociationValue(Association association) {
      Entity entity = association.ReferencedEntity;
      if (!OrderHasBeenSet(entity)) {
        if (entity.FindValue<int>(DataGenLibrary.QUANTITY, out int quantity) && quantity > 0) 
          GenerateForEntity(entity);
        else {
          if (association.IsMandatory)
            throw new Exception(string.Format("Association {0} is mandatory, but Entity {1} does not specify {2} for data generation",
              association, entity.Name, DataGenLibrary.UNIQUE));
        }
      }
      return new MemberAndValue() {
        Member = association,
        Value = _entityInfo[entity].RandomExistingId(_random, association),
      };
    }
    #endregion

    #region Utilities

    private bool OrderHasBeenSet(Entity entity) {
      return _entityInfo[entity].OrderIndex != null;
    }

    private void SetEntityOrderIfFirstTime(Entity entity) {
      if (!OrderHasBeenSet(entity))
        _entityInfo[entity].OrderIndex = _tableOrderIndex++;
    }

    private Row CreateAndStoreRow(Entity entity) {
      EntityInfo info = _entityInfo[entity];
      Row row = new Row() {
        Entity = entity,
        Id = info.NextId++,
      };
      info.Rows.Add(row);
      return row;
    }

    private int GetCount(Association association) {
      if (association.IsMany) {
        SqlRange range = association.FindValue<SqlRange>(DataGenLibrary.QUANTITY) ?? DEFAULT_ASSOCIATION_RANGE;
        return range.GetRandom(_random);
      } else
        return 1;   // TODO: Consider probability if not mandatory
    }

    private int GetCount(Entity entity) {
      if (!entity.FindValue<int>(DataGenLibrary.QUANTITY, out int quantity))
        return 0;
      return TEST_REDUCED_NUMBER_OF_ROWS ?? quantity;
    }
    #endregion
  }
}

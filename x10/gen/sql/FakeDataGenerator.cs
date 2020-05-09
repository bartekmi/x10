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

namespace x10.gen.sql {
  public class FakeDataGenerator {

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
      internal int OrderIndex;

      internal int RandomExistingId(Random random) {
        if (Rows.Count == 0)
          throw new Exception("No rows to choose from");
        return random.Next(1, NextId);
      }
    }

    private static readonly SqlRange DEFAULT_ASSOCIATION_RANGE = SqlRange.Parse("0..3");

    private readonly Random _random;
    private readonly Dictionary<Entity, EntityInfo> _entityInfo;
    private readonly HashSet<Entity> _unprocessedRootEntities;
    private int _tableOrderIndex;

    public static void Generate(IEnumerable<Entity> entities, Random random, string filename) {
      using (TextWriter writer = new StreamWriter(filename))
        GeneratePrivate(entities, random, writer);
    }

    public static string GenerateIntoString(IEnumerable<Entity> entities, Random random) {
      using (TextWriter writer = new StringWriter()) {
        GeneratePrivate(entities, random, writer);
        return writer.ToString();
      }
    }

    public static void GeneratePrivate(IEnumerable<Entity> entities, Random random, TextWriter writer) {
      IEnumerable<Entity> relevantEntities = entities.Where(x => !SqlSchemaGenerator.Ignore(x));
      FakeDataGenerator generator = new FakeDataGenerator(relevantEntities, random);
      generator.Generate();

      DumpData(generator._entityInfo, writer);
    }

    #region Write to File

    // Sample:

    // INSERT INTO my_table (id, col1, col2, ...) VALUES
    // (1, val1, val2, ...),
    // ...
    // (N, val1, val2, ...);
    private static void DumpData(Dictionary<Entity, EntityInfo> entityInfos, TextWriter writer) {
      IEnumerable<EntityInfo> infos = entityInfos.Values
        .Where(x => x.Rows.Count > 0)
        .OrderBy(x => x.OrderIndex);

      foreach (EntityInfo entityInfo in infos) {
        // Write the INSERT statement
        Entity entity = entityInfo.Entity;
        IEnumerable<string> dbColumnNames = entity.Members.Select(x => SqlSchemaGenerator.GetDbColumnName(x));

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
    private FakeDataGenerator(IEnumerable<Entity> entities, Random random) {
      IEnumerable<Entity> rootLevelEntities = entities.Where(x => GetCount(x) > 0);
      _unprocessedRootEntities = new HashSet<Entity>(rootLevelEntities);
      _random = random;

      _entityInfo = entities.ToDictionary(x => x, (x) => new EntityInfo() {
        Context = DataGenerationContext.CreateContext(_random, x),
        Entity = x,
      });
    }

    private void Generate() {
      while (_unprocessedRootEntities.Count > 0) {          // Until no more root-level entities
        Entity entity = _unprocessedRootEntities.First();
        GenerateForEntity(entity);
      }
    }

    private void GenerateForEntity(Entity entity) {
      // Ensure all dependencies are present
      foreach (Entity dependency in GetImmediateDependencies(entity))
        if (_unprocessedRootEntities.Contains(dependency))
          GenerateForEntity(dependency);

      // Generate all rows
      _entityInfo[entity].OrderIndex = _tableOrderIndex++;    // Ensures correct dependency order when printing
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
          GenerateRow(association.ReferencedEntity, parent);
      }
    }

    private Row GenerateSingleRow(Entity entity, Row parent) {
      Row row = CreateAndStoreRow(entity);

      Association linkToParent = null;
      if (parent != null) {
        IEnumerable<Association> parentTypeAssociations
          = entity.Associations.Where(x => x.ReferencedEntity == parent.Entity);
        if (parentTypeAssociations.Count() != 1)
          throw new Exception(string.Format("Entity {0} referenced by Entity {1} must have exactly one association of that type, but has {2}",
            entity.Name, parent.Entity.Name, parentTypeAssociations.Count()));
        linkToParent = parentTypeAssociations.Single();
      }

      DataGenerationContext context = _entityInfo[entity].Context;
      DataFileRow externalRow = context.GetRandomExternalFileRow();

      foreach (Member member in entity.Members)
        if (member is X10Attribute x10Attr)
          row.Values.Add(AtomicDataGenerator.Generate(_random, context, x10Attr, externalRow));
        else if (member is Association association)
          if (association == linkToParent)
            row.Values.Add(new MemberAndValue() {
              Member = association,
              Value = parent.Id,
            });
          else {
            // Must be a derived attribute... Ignore
          }
      return row;
    }


    private MemberAndValue CreateAssociationValue(Association association) {
      return new MemberAndValue() {
        Member = association,
        Value = _entityInfo[association.ReferencedEntity].RandomExistingId(_random),
      };
    }
    #endregion

    #region Utilities

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
        SqlRange range = association.FindValue<SqlRange>("datagen_quantity") ?? DEFAULT_ASSOCIATION_RANGE;
        return range.GetRandom(_random);
      } else
        return 1;   // TODO: Consider probability if not mandatory
    }

    private int GetCount(Entity entity) {
      if (!entity.FindValue<int>("datagen_quantity", out int quantity))
        return 0;
      return quantity;
    }

    private IEnumerable<Entity> GetImmediateDependencies(Entity entity) {
      IEnumerable<Entity> dependencies = entity.Associations
        .Where(x => !x.Owns)
        .Select(x => x.ReferencedEntity)
        .Distinct();

      string depsList = string.Join(", ", dependencies.Select(x => x.Name));
      Console.WriteLine("Deps for {0}: {1}", entity.Name, depsList);
      return dependencies;
    }
    #endregion
  }
}

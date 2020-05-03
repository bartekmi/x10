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
    }

    private class EntityInfo {
      internal int NextId = 1;
      internal List<Row> Rows = new List<Row>();
      internal DataGenerationContext Context;

      internal int RandomExistingId(Random random) {
        if (Rows.Count == 0)
          throw new Exception("No rows to choose from");
        return random.Next(1, NextId);
      }
    }

    private static readonly Range DEFAULT_ASSOCIATION_RANGE = Range.Parse("0..3");

    private readonly Random _random;
    private readonly Dictionary<Entity, EntityInfo> _entityInfo;
    private readonly HashSet<Entity> _unprocessedRootEntities;

    public static void Generate(IEnumerable<Entity> entities, string filename) {
      IEnumerable<Entity> relevantEntities = entities.Where(x => !SqlSchemaGenerator.Ignore(x));
      FakeDataGenerator generator = new FakeDataGenerator(relevantEntities);
      generator.Generate();

      DumpData(generator._entityInfo, filename);
    }

    private static void DumpData(Dictionary<Entity, EntityInfo> entityInfo, string filename) {
      using (TextWriter writer = new StreamWriter(filename)) {
        // TODO... Print content of all rows in appropriate order
      }
    }

    private FakeDataGenerator(IEnumerable<Entity> entities) {
      _random = new Random();
      _entityInfo = entities.ToDictionary(x => x, (x) => new EntityInfo() {
        Context = DataGenerationContext.CreateContext(_random, x),
      });

      IEnumerable<Entity> rootLevelEntities = entities.Where(x => GetCount(x) > 0);
      _unprocessedRootEntities = new HashSet<Entity>(rootLevelEntities);
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

      foreach (Member member in entity.Members)
        if (member is X10Attribute x10Attr)
          row.Values.Add(AtomicDataGenerator.Generate(_random, _entityInfo[entity].Context, x10Attr));
        else if (member is Association association)
          if (association == linkToParent)
            row.Values.Add(new MemberAndValue() {
              Member = association,
              Value = parent.Id,
            });
          else
            row.Values.Add(CreateAssociationValue(association));
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
        Range range = association.FindValue<Range>("datagen_quantity") ?? DEFAULT_ASSOCIATION_RANGE;
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
      return entity.Associations.Where(x => !x.Owns).Select(x => x.ReferencedEntity);
    }
    #endregion

    // Sample:

    // INSERT INTO my_table (id, col1, col2, ...) VALUES
    // (1, val1, val2, ...),
    // ...
    // (N, val1, val2, ...);
    //private static void GenerateDataForTable(TextWriter writer, Entity entity, ReverseAssociationCalculator reverseAssociations) {
  }
}

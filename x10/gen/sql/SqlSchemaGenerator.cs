using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;

using x10.model;
using x10.model.definition;
using System.Linq;
using x10.model.metadata;
using x10.utils;

namespace x10.gen.sql {
  public static class SqlSchemaGenerator {

    #region Top Level
    private enum ColumnType {
      Attribute,
      ForwardAssociation,
      ReverseAssociation,
    }

    public static bool Ignore(Entity entity) {
      return entity.FindBoolean(DataGenLibrary.NO_SQL_SCHEMA, false) ||
        entity.IsAbstract ||
        entity.Name == ModelValidationUtils.CONTEXT_ENTITY_NAME;
    }

    public static void Generate(IEnumerable<Entity> entities, string filename) {
      entities = entities.Where(x => !Ignore(x));

      ReverseAssociationCalculator reverse = new ReverseAssociationCalculator(entities);

      using (TextWriter writer = new StreamWriter(filename)) {
        // Generate Tables
        writer.WriteLine("------------------------ Tables ------------------------------");
        foreach (Entity entity in entities)
          GenerateTable(writer, entity, reverse);

        // Generate FK Constraints - thought the original idea was to do this at same time as column creation,
        // this becomes impossible if there are circular dependencies among the tables.
        writer.WriteLine();
        writer.WriteLine();
        writer.WriteLine("------------------------ Foreign Key Constraints ------------------------------");
        foreach (Entity entity in entities) 
          GenerateFkConstraints(writer, entity, reverse);
      }
    }
    #endregion

    #region Create Table
    // Sample:

    // CREATE TABLE table_name(
    //   user_id serial PRIMARY KEY,
    //   username VARCHAR UNIQUE NOT NULL,
    // );
    private static void GenerateTable(TextWriter writer, Entity entity, ReverseAssociationCalculator reverseAssociations) {

      writer.WriteLine("CREATE TABLE \"{0}\" (", Snake(entity.Name));
      writer.WriteLine("  id serial PRIMARY KEY,");

      IEnumerable<ReverseOwner> reverses = reverseAssociations.Get(entity);

      IEnumerable<object> members = entity.RegularAttributes.Cast<object>()
        .Concat(entity.Associations.Where(x => !x.IsMany))
        .Concat(reverses);


      ColumnType? previousType = null;

      foreach (object member in members) {
        ColumnType type;
        if (member is X10Attribute attribute) {
          type = ColumnType.Attribute;
          WriteSeparator(writer, previousType, type);
          WriteAttribute(writer, attribute);
        } else if (member is Association association) {
          if (IsDefinedInBothDirections(reverses, association)) {
            // If association is defined from both ends, this would otherwise cause multiple columns
            // to be generated. We give preference to the one created in the reverse direction (thus skipping generation here)
            // because the reverse one will ensure a 'not null' clause.
            continue;
          } else {
            type = ColumnType.ForwardAssociation;
            WriteSeparator(writer, previousType, type);
            WriteForwardAssociation(writer, association);
          }
        } else if (member is ReverseOwner reverse) {
          type = ColumnType.ReverseAssociation;
          WriteSeparator(writer, previousType, type);
          WriteReverseAssociation(writer, reverse);
        } else
          throw new Exception("Unexpected member type: " + member.GetType().Name);

        // Write (or don't write) comma
        if (member != members.Last())
          writer.Write(',');
        writer.WriteLine();

        previousType = type;
      }

      writer.WriteLine(");");
      writer.WriteLine();
    }

    private static void WriteSeparator(TextWriter writer, ColumnType? previousType, ColumnType type) {
      if (previousType != null && type != previousType)
        writer.WriteLine();
    }


    private static void WriteAttribute(TextWriter writer, X10Attribute attribute) {
      writer.Write("  {0} {1}{2}NULL",
        Snake(attribute.Name),
        GetType(attribute.DataType),
        attribute.IsMandatory ? " NOT " : " ");
    }

    private static void WriteForwardAssociation(TextWriter writer, Association association) {
      writer.Write("  {0}_id INTEGER{1}NULL",
        Snake(association.Name),
        association.IsMandatory ? " NOT " : " ");
    }

    private static void WriteReverseAssociation(TextWriter writer, ReverseOwner reverse) {
      writer.Write("  {0}_id INTEGER NOT NULL",
        Snake(reverse.ActualOwner.Name));
    }

    private static string Snake(string camelCase) {
      return NameUtils.CamelCaseToSnakeCase(camelCase);
    }

    private static string QuotedSnake(string camelCase) {
      string snakeCase = NameUtils.CamelCaseToSnakeCase(camelCase);
      return '"' + snakeCase + '"';
    }

    // https://www.postgresql.org/docs/9.5/datatype.html
    private static string GetType(DataType dataType) {
      if (dataType == DataTypes.Singleton.Boolean) return "BOOLEAN";
      if (dataType == DataTypes.Singleton.Date) return "DATE";
      if (dataType == DataTypes.Singleton.Float) return "DOUBLE PRECISION";
      if (dataType == DataTypes.Singleton.Integer) return "INTEGER";
      if (dataType == DataTypes.Singleton.String) return "VARCHAR";
      if (dataType == DataTypes.Singleton.Timestamp) return "TIMESTAMP";
      if (dataType == DataTypes.Singleton.Money) return "MONEY";
      if (dataType is DataTypeEnum) return "VARCHAR";

      throw new Exception("Unknown data type: " + dataType.Name);
    }
    #endregion

    #region FK Constraints
    // Sample: 

    // ALTER TABLE child_table
    // ADD CONSTRAINT constraint_name FOREIGN KEY(c1) REFERENCES parent_table(p1);
    private static void GenerateFkConstraints(TextWriter writer, Entity entity, ReverseAssociationCalculator reverseAssociations) {
      IEnumerable<Association> associations = entity.Associations.Where(x => !x.IsMany);
      IEnumerable<Association> manyAssociations = entity.Associations.Where(x => x.IsMany);
      IEnumerable<ReverseOwner> reverses = reverseAssociations.Get(entity);

      if (associations.Count() == 0 && manyAssociations.Count() == 0)
        return;

      writer.WriteLine("-- Related to Table " + entity.Name);

      foreach (Association association in associations) {
        if (IsDefinedInBothDirections(reverses, association)) {
          // Similar reasoning as the same condition in table generation - we need this to prevent generating
          // duplicate constraints for the same association
        } else {
          writer.WriteLine(string.Format("ALTER TABLE \"{0}\" ADD CONSTRAINT {0}_{1}_fkey FOREIGN KEY({1}_id) REFERENCES \"{2}\"(id);",
          Snake(entity.Name),
          Snake(association.Name),
          Snake(association.ReferencedEntity.Name)));
        }
      }

      foreach (Association association in manyAssociations)
        writer.WriteLine(string.Format("ALTER TABLE \"{0}\" ADD CONSTRAINT {0}_{1}_fkey FOREIGN KEY({1}_id) REFERENCES \"{1}\"(id);",
          Snake(association.ReferencedEntity.Name),
          Snake(entity.Name)));

      writer.WriteLine();
    }
    #endregion

    #region Utilities
    private static bool IsDefinedInBothDirections(IEnumerable<ReverseOwner> reverses, Association association) {
      return reverses.Any(x => x.ActualOwner == association.ReferencedEntity);
    }
    #endregion
  }
}

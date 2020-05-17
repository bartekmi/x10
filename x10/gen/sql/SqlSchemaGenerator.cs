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
    public static void Generate(IEnumerable<Entity> entities, string filename) {
      using (TextWriter writer = new StreamWriter(filename))
        Generate(entities, writer);
    }

    public static string GenerateIntoString(IEnumerable<Entity> entities) {
      using (TextWriter writer = new StringWriter()) {
        Generate(entities, writer);
        return writer.ToString();
      }
    }

    public static void Generate(IEnumerable<Entity> entities, TextWriter writer) {

      DeclaredColumnsCalculator reverse = new DeclaredColumnsCalculator(entities);
      entities = reverse.GetRealEntities();

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
    #endregion

    #region Create Table
    // Sample:

    // CREATE TABLE table_name(
    //   user_id serial PRIMARY KEY,
    //   username VARCHAR UNIQUE NOT NULL,
    // );
    private static void GenerateTable(TextWriter writer, Entity entity, DeclaredColumnsCalculator columnCalculator) {

      writer.WriteLine("CREATE TABLE \"{0}\" (", GetTableName(entity));
      writer.WriteLine("  id serial PRIMARY KEY,");

      List<MemberAndOwner> declaredColumns = columnCalculator.GetDeclaredColumns(entity).ToList();

      ColumnType? previousType = null;

      foreach (MemberAndOwner member in declaredColumns) {
        ColumnType type = member.Type;
        WriteSeparator(writer, previousType, type);

        switch (type) {
          case ColumnType.Attribute:
            WriteAttribute(writer, member);
            break;
          case ColumnType.ForwardAssociation:
            WriteForwardAssociation(writer, member);
            break;
          case ColumnType.ReverseAssociation:
            bool hasMultipleReverseAssociations = columnCalculator.GetReverseAssociations(entity).Count() > 1;
            WriteReverseAssociation(writer, member, hasMultipleReverseAssociations);
            break;
          default:
            throw new Exception("Unexpected member type: " + type);
        }

        // Write (or don't write) comma
        if (member != declaredColumns.Last())
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


    private static void WriteAttribute(TextWriter writer, MemberAndOwner memberAndOwner) {
      X10Attribute attribute = memberAndOwner.Attribute;

      writer.Write("  {0} {1}{2}NULL",
        GetDbColumnName(memberAndOwner),
        GetType(attribute.DataType),
        attribute.IsMandatory ? " NOT " : " ");
    }

    private static void WriteForwardAssociation(TextWriter writer, MemberAndOwner forward) {
      writer.Write("  {0} INTEGER{1}NULL",
        GetDbColumnName(forward),
        forward.Association.IsMandatory ? " NOT " : " ");
    }

    private static void WriteReverseAssociation(TextWriter writer, MemberAndOwner reverse, bool hasMultipleReverseAssociations) {
      writer.Write("  {0} INTEGER{1}NULL",
        GetDbColumnName(reverse),
        hasMultipleReverseAssociations ? " " : " NOT ");
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
    private static void GenerateFkConstraints(TextWriter writer, Entity entity, DeclaredColumnsCalculator columnCalculator) {
      IEnumerable<MemberAndOwner> forwardAssociations = columnCalculator.GetForwardAssociations(entity);
      IEnumerable<MemberAndOwner> reverseAssociations = columnCalculator.GetReverseAssociations(entity);

      if (forwardAssociations.Count() == 0 && reverseAssociations.Count() == 0)
        return;

      writer.WriteLine("-- Related to Table " + entity.Name);

      foreach (MemberAndOwner association in forwardAssociations) {
        writer.WriteLine(string.Format("ALTER TABLE \"{0}\" ADD CONSTRAINT {0}_{1}_fkey FOREIGN KEY({1}) REFERENCES \"{2}\"(id);",
        GetTableName(entity),
        GetDbColumnName(association),
        GetTableName(association.Association.ReferencedEntity)));
      }

      foreach (MemberAndOwner association in reverseAssociations)
        writer.WriteLine(string.Format("ALTER TABLE \"{0}\" ADD CONSTRAINT {0}_{1}_fkey FOREIGN KEY({1}) REFERENCES \"{2}\"(id);",
          GetTableName(entity),
          GetDbColumnName(association),
          GetTableName(association.ActualOwner)));

      writer.WriteLine();
    }
    #endregion

    #region Utilities

    internal static string GetDbColumnName(MemberAndOwner memberAndOwner) {
      string memberName = memberAndOwner.Member.Name;

      switch (memberAndOwner.Type) {
        case ColumnType.Attribute:
          return NameUtils.CamelCaseToSnakeCase(memberName);
        case ColumnType.ForwardAssociation:
          return NameUtils.CamelCaseToSnakeCase(memberName) + "_id";
        case ColumnType.ReverseAssociation:
          return GetTableName(memberAndOwner.ActualOwner) + "_id";
        default:
          throw new Exception("Unexpected type: " + memberAndOwner.Type);
      }
    }

    internal static string GetTableName(Entity entity) {
      return NameUtils.CamelCaseToSnakeCase(entity.Name);
    }
    #endregion
  }
}

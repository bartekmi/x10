using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using FileInfo = x10.parsing.FileInfo;
using x10.compiler;
using x10.model;
using x10.model.definition;
using x10.model.metadata;
using x10.ui.composition;
using x10.utils;
using x10.ui.platform;
using x10.parsing;
using x10.formula;
using x10.gen.wpf;

namespace x10.gen.hotchoc {
  public class HotchocCodeGenerator : CodeGenerator {
    public override void Generate(ClassDefX10 classDef) { }

    public override void Generate(Entity entity) {
      Begin(entity.TreeElement.FileInfo, ".cs");

      WriteLine(0, "using System;");
      WriteLine(0, "using System.Collections.Generic;");
      WriteLine();
      WriteLine(0, "using HotChocolate;");
      WriteLine();
      WriteLine(0, "namespace x10.hotchoc.Entities {");

      GenerateEnums(entity);
      GenerateMainEntity(entity);

      WriteLine(0, "}");

      End();
    }

    private void GenerateEnums(Entity entity) {
      IEnumerable<DataTypeEnum> enums = FindLocalEnums(entity);
      if (enums.Count() == 0)
        return;

      WriteLine(0, "// Enums");

      foreach (DataTypeEnum theEnum in enums) 
        GenerateEnum(1, theEnum);

      WriteLine();
    }

    #region Generate Main Entity
    private void GenerateMainEntity(Entity entity) {
      WriteLine(1, "/// <summary>");
      WriteLine(1, "/// {0}", entity.Description);
      WriteLine(1, "/// </summary>");
      WriteLine(1, "public class {0} : EntityBase {", entity.Name);

      GenerateRegularAttributes(entity);
      GenerateToStringRepresentation(entity);
      GenerateAssociations(entity);

      WriteLine(1, "}");
    }

    private void GenerateRegularAttributes(Entity entity) {
      WriteLine(2, "// Regular Attributes");

      foreach (X10RegularAttribute attribute in entity.RegularAttributes) {
        WriteLine(2, "[GraphQLNonNullType]");
        WriteLine(2, "public {0} {1} { get; set; } }", 
          DataType(attribute),
          PropName(attribute));
      }

      WriteLine();
    }

    private void GenerateToStringRepresentation(Entity entity) {
      WriteLine(2, "// To String Representation");
      WriteLine(2, "[GraphQLNonNullType]");
      
      string formula = entity.StringRepresentation == null ?
        string.Format("\"{0}: \" + Dbid", entity.Name) : 
        ExpressionToString(entity.StringRepresentation);

      WriteLine(2, "public string? ToStringRepresentation => {0}", formula);

      WriteLine();
    }

    private void GenerateAssociations(Entity entity) {
      WriteLine(2, "// Associations");

      foreach (Association association in entity.Associations) {
        Entity refedEntity = association.ReferencedEntity;
        string propName = PropName(association);

        if (association.IsMany) {
          WriteLine(2, "[GraphQLNonNullType]");
          WriteLine(2, "public List<{0}>? {1} { get; set; } }", refedEntity.Name, propName);
        } else {
          if (association.IsMany)
            WriteLine(2, "[GraphQLNonNullType]");
          WriteLine(2, "public {0}? {1} { get; set; } }", refedEntity.Name, propName);
        }
      }

      WriteLine();
    }
    #endregion

    #region Generate Enum Files
    public override void GenerateEnumFile(FileInfo fileInfo, IEnumerable<DataTypeEnum> enums) {
      Begin(fileInfo, ".cs");

      WriteLine(0, "using wpf_lib.lib.attributes;");
      WriteLine();

      foreach (DataTypeEnum anEnum in enums)
        GenerateEnum(0, anEnum);

      End();
    }
    #endregion

    #region Utils
    private static string PropName(Member member) {
      return NameUtils.CapitalizeFirstLetter(member.Name);
    }

    private static string DataType(X10Attribute attribute) {
      DataType dataType = attribute.DataType;

      if (dataType == DataTypes.Singleton.Boolean) return "bool";
      if (dataType == DataTypes.Singleton.Date) return "DateTime?";
      if (dataType == DataTypes.Singleton.Float) return "double?";
      if (dataType == DataTypes.Singleton.Integer) return "int?";
      if (dataType == DataTypes.Singleton.String) return "string?";
      if (dataType == DataTypes.Singleton.Timestamp) return "DateTime?";
      if (dataType == DataTypes.Singleton.Money) return "double?";
      if (dataType is DataTypeEnum enumType) return EnumToName(enumType) + "?";

      throw new Exception("Unknown data type: " + dataType.Name);
    }

    private static string EnumToName(DataTypeEnum enumType) {
      return enumType.Name + "Enum";
    }

    private static string ExpressionToString(ExpBase expression) {
      if (expression == null)
        return "EXPRESSION MISSING";

      using StringWriter writer = new StringWriter();

      WpfFormulaWriter formulaWriterVisitor = new WpfFormulaWriter(writer, false);
      expression.Accept(formulaWriterVisitor);
      return writer.ToString();
    }

    public void GenerateEnum(int level, DataTypeEnum theEnum) {
      WriteLine(level, "public enum {0} {", WpfGenUtils.EnumToName(theEnum));

      foreach (EnumValue enumValue in theEnum.EnumValues) {
        if (enumValue.Label != null)
          WriteLine(level + 1, "[Label(\"{0}\")]", enumValue.Label);
        if (enumValue.IconName != null)
          WriteLine(level + 1, "[Icon(\"{0}\")]", enumValue.IconName);
        WriteLine(level + 1, "{0},", enumValue.ValueUpperCased);
      }

      WriteLine(level, "}");
      WriteLine();
    }
    #endregion
  }
}
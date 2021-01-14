using System;
using System.Collections.Generic;
using System.Linq;

using FileInfo = x10.parsing.FileInfo;
using x10.model.definition;
using x10.model.metadata;

namespace x10.gen.react {
  public partial class ReactCodeGenerator : CodeGenerator {
    public override void Generate(Entity entity) {
      FileInfo fileInfo = entity.TreeElement.FileInfo;
      Begin(fileInfo, ".js");

      GenerateFileHeader();
      WriteLine(0, "import { v4 as uuid } from 'uuid';");
      WriteLine();
      WriteLine(0, "import { DBID_LOCALLY_CREATED } from 'react_lib/constants';");
      WriteLine();
      InsertImportsPlaceholder();

      GenerateType(entity);
      GenerateDefaultEntity(entity);
      GenerateEnums(entity);
      GenerateDerivedAttributes(entity);

      End();
    }

    #region Generate Type Definition
    private void GenerateType(Entity model) {
      WriteLine(0, "// Type Definition");
      WriteLine(0, "export type {0} = {{|", model.Name);

      WriteLine(1, "+id: string,");

      foreach (Member member in model.Members)
        if (member is X10DerivedAttribute) {
          // Do not generate derived members
        } else {
          WriteLine(1, "+{0}: {1},", member.Name, GetType(member));
          if (member is Association association) {
            Entity entity = association.ReferencedEntity;
            ImportsPlaceholder.ImportType(entity.Name, entity);
          }
        }
      WriteLine(0, "|}};");
      WriteLine();
      WriteLine();
    }
    #endregion

    #region Generate Default Entity
    private void GenerateDefaultEntity(Entity model) {
      WriteLine(0, "// Create Default Function");
      WriteLine(0, "export function {0}(): {1} {", ReactCodeGenerator.CreateDefaultFuncName(model), model.Name);
      WriteLine(1, "return {");

      WriteLine(2, "id: uuid(),");

      foreach (Member member in model.Members)
        if (member is X10DerivedAttribute) {
          // Do not generate derived members
        } else {
          string defaultValue = GetDefaultValue(member, ImportsPlaceholder);
          if (defaultValue == null) {
            defaultValue = "null";
            if (member.IsMandatory)
              WriteLine(2, "// $FlowExpectedError Required field, but no default value");
          }
          WriteLine(2, "{0}: {1},", member.Name, defaultValue);
        }

      WriteLine(1, "};");
      WriteLine(0, "}");
      WriteLine();
      WriteLine();
    }

    private string GetDefaultValue(Member member, ImportsPlaceholder importsPlaceholder) {
      if (member is Association association) {
        if (association.IsMany)
          return "[]";
        else {
          Entity entity = association.ReferencedEntity;
          string funcName = "createDefault" + entity.Name;
          importsPlaceholder.Import(funcName, entity);
          return funcName + "()";
        }
      } else if (member is X10RegularAttribute attribute) {
        if (attribute.IsId)
          return "DBID_LOCALLY_CREATED";

        object defaultValue = attribute.DefaultValue;
        DataType dataType = attribute.DataType;

        if (defaultValue == null) {
          if (dataType == DataTypes.Singleton.Boolean) return "false";
          if (dataType == DataTypes.Singleton.String) return "''";
        } else
          return TypedLiteralToString(defaultValue, dataType as DataTypeEnum);
      }

      return null;
    }
    #endregion

    #region Generate Enums
    private void GenerateEnums(Entity entity) {
      IEnumerable<DataTypeEnum> enums = FindLocalEnums(entity);
      if (enums.Count() == 0)
        return;

      WriteLine(0, "// Enums");

      foreach (DataTypeEnum theEnum in enums) {
        GeneratePairs(theEnum);
        GenerateEnumType(theEnum);
      }

      WriteLine();
      WriteLine();
    }

    private void GeneratePairs(DataTypeEnum theEnum) {
      WriteLine(0, "export const {0} = [", EnumToPairsConstant(theEnum));

      foreach (EnumValue enumValue in theEnum.EnumValues) {
        WriteLine(1, "{");
        WriteLine(2, "value: '{0}',", ToEnumValueString(enumValue.Value));
        WriteLine(2, "label: '{0}',", enumValue.EffectiveLabel);
        if (enumValue.IconName != null)
          WriteLine(1, "icon: '{0}'", enumValue.IconName);
        WriteLine(1, "},");
      }

      WriteLine(0, "];");
      WriteLine();
    }

    private void GenerateEnumType(DataTypeEnum theEnum) {
      IEnumerable<string> enumStrings =
        theEnum.AvailableValuesAsStrings.Select(x => string.Format("'{0}'", ToEnumValueString(x)));

      WriteLine(0, "export type {0} = {1};",
        EnumToName(theEnum),
        string.Join(" | ", enumStrings));

      WriteLine();
    }
    #endregion

    #region Generate Derived Attributes
    private void GenerateDerivedAttributes(Entity entity) {
      if (entity.DerivedAttributes.Count() == 0)
        return;

      WriteLine(0, "// Derived Attribute Functions");

      foreach (X10DerivedAttribute attribute in entity.DerivedAttributes) {
        PushSourceVariableName(VariableName(entity, false));

        WriteLine(0, "export function {0}({1}: {2}): {3} {",
          DerivedAttrFuncName(attribute),
          SourceVariableName,
          entity.Name,
          GetType(attribute));

        if (attribute.Expression.UsesContext) {
          WriteLine(1, "const appContext = React.useContext(AppContext);");
          ImportsPlaceholder.ImportAppContext();
        }

        WriteLine(1, "return {0};", ExpressionToString(attribute.Expression));
        WriteLine(0, "}");

        PopSourceVariableName();
      }

      WriteLine();
      WriteLine();
    }
    #endregion

    #region Utilities

    private string GetType(Member member) {
      if (member is Association association) {
        string refedEntityName = association.ReferencedEntity.Name;
        if (association.IsMany)
          return string.Format("$ReadOnlyArray<{0}>", refedEntityName);
        else
          // Generate mandatory even if not mandatory. Non-mandatory element cause all kinds of 
          // problems with typing, not to mention the fact that we would need to create the optional
          // association object at just the right type during editing
          return refedEntityName;
      } else if (member is X10Attribute attribute) {
        string optionalIndicator = IsMandatory(attribute) ? "" : "?";
        return optionalIndicator + GetAtomicFlowType(attribute.DataType);
      } else
        throw new NotImplementedException("Unknown member type: " + member.GetType());
    }

    private bool IsMandatory(X10Attribute attribute) {
      // If an attribute is read-only, editing is not an issue, so simply reflect the
      // "mandatory-ness" of the attribute
      if (attribute.IsId)
        return attribute.IsMandatory;

      DataType dataType = attribute.DataType;

      // The mandatory/optional decision is made based on the usual UI used to represent
      // the data type. It is assumed that booleans are represented by CheckBoxes and
      // Strings are represented by TextInput's or similar where the empty string represents
      // no user input
      if (dataType == DataTypes.Singleton.Boolean) return true;
      if (dataType == DataTypes.Singleton.String) return true;

      // All UI element used to represent these types can have a null state. For example,
      // a DateInput UI can have a state where no value has been specified, and so for all
      // the other types.
      if (dataType == DataTypes.Singleton.Date) return false;
      if (dataType == DataTypes.Singleton.Float) return false;
      if (dataType == DataTypes.Singleton.Integer) return false;
      if (dataType == DataTypes.Singleton.Timestamp) return false;
      if (dataType == DataTypes.Singleton.Money) return false;
      if (dataType is DataTypeEnum enumType) return false;

      throw new NotImplementedException("Unknown data type: " + dataType.Name);
    }

    private string GetAtomicFlowType(DataType dataType) {
      if (dataType == DataTypes.Singleton.Boolean) return "boolean";
      if (dataType == DataTypes.Singleton.Date) return "string";
      if (dataType == DataTypes.Singleton.Float) return "number";
      if (dataType == DataTypes.Singleton.Integer) return "number";
      if (dataType == DataTypes.Singleton.String) return "string";
      if (dataType == DataTypes.Singleton.Timestamp) return "string";
      if (dataType == DataTypes.Singleton.Money) return "number";
      if (dataType is DataTypeEnum enumType) return EnumToName(enumType);

      throw new NotImplementedException("Unknown data type: " + dataType.Name);
    }
    #endregion
  }
}

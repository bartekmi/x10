﻿using System;
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
      OutputPlaceholder importsPlaceholder = CreatePlaceholder(0);
      WriteLine();

      GenerateType(entity, importsPlaceholder);
      GenerateDefaultEntity(entity, importsPlaceholder);
      GenerateEnums(entity);
      GenerateDerivedAttributes(entity);

      End();
    }

    #region Generate Type Definition
    private void GenerateType(Entity model, OutputPlaceholder importsPlaceholder) {
      WriteLine(0, "// Type Definition");
      WriteLine(0, "export type {0} = {{|", model.Name);

      WriteLine(1, "+id: string,");
      WriteLine(1, "+dbid: number,");

      foreach (Member member in model.Members)
        if (member is X10DerivedAttribute) {
          // Do not generate derived members
        } else {
          WriteLine(1, "+{0}: {1},", member.Name, GetType(member));
          if (member is Association association) {
            Entity entity = association.ReferencedEntity;
            importsPlaceholder.WriteLine("import { type {0} } from '{1}'", entity.Name, ImportPath(entity));
          }
        }
      WriteLine(0, "|}};");
      WriteLine();
      WriteLine();
    }

    private string GetType(Member member) {
      // Strings are never marked as optional because, at the very least, the have default value of ""
      bool isString = member is X10RegularAttribute regular && regular.DataType == DataTypes.Singleton.String;
      string optionalIndicator = member.IsMandatory || isString ? "" : "?";

      if (member is Association association) {
        string refedEntityName = association.ReferencedEntity.Name;
        if (association.IsMany)
          return string.Format("$ReadOnlyArray<{0}>", refedEntityName);
        else
          return optionalIndicator + refedEntityName;
      } else if (member is X10Attribute attribute) {
        DataType dataType = attribute.DataType;
        return optionalIndicator + GetAtomicFlowType(dataType);
      }

      return "null,";
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

      throw new Exception("Unknown data type: " + dataType.Name);
    }
    #endregion

    #region Generate Default Entity
    private void GenerateDefaultEntity(Entity model, OutputPlaceholder importsPlaceholder) {
      WriteLine(0, "// Create Default Function");
      WriteLine(0, "export function createDefault{0}(): {0} {", model.Name);
      WriteLine(1, "return {");

      WriteLine(2, "id: uuid(),");
      WriteLine(2, "dbid: DBID_LOCALLY_CREATED,");

      foreach (Member member in model.Members)
        if (member is X10DerivedAttribute) {
          // Do not generate derived members
        } else {
          string defaultValue = GetDefaultValue(member, importsPlaceholder);
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

    private string GetDefaultValue(Member member, OutputPlaceholder importsPlaceholder) {
      if (member is Association association) {
        if (association.IsMany)
          return "[]";
        else
          if (association.IsMandatory) {
            Entity entity = association.ReferencedEntity;
            string funcName = "createDefault" + entity.Name;
            importsPlaceholder.WriteLine("import { {0} } from '{1}'", funcName, ImportPath(entity));
            return funcName + "()";
          }
      } else if (member is X10RegularAttribute attribute) {
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
        string variableName = VariableName(entity, false);
        WriteLine(0, "export function {0} ({1}: {2}): {3} {", 
          DerivedAttrFuncName(attribute), 
          variableName, 
          entity.Name,
          GetType(attribute));

        WriteLine(1, "return {0};", ExpressionToString(attribute.Expression, variableName));
        WriteLine(0, "}");
      }

      WriteLine();
      WriteLine();
    }
    #endregion
  }
}

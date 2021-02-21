using System;
using System.Collections.Generic;
using System.Linq;

using FileInfo = x10.parsing.FileInfo;
using x10.model.definition;
using x10.model.metadata;
using x10.formula;
using x10.utils;
using x10.gen.react.placeholder;

namespace x10.gen.react.generate {
  public partial class ReactCodeGenerator : CodeGenerator {

    #region Top Level
    public override void GenerateCommon() { }

    public override void Generate(Entity entity) {
      FileInfo fileInfo = entity.TreeElement.FileInfo;
      bool isContext = entity.IsContext;

      Begin(fileInfo, ".js");

      GenerateFileHeader();
      WriteLine();
      InsertImportsPlaceholder();

      GenerateType(entity, isContext);
      GenerateEnums(entity);
      GenerateDerivedAttributes(entity);

      if (!isContext) {
        GenerateDefaultEntity(entity);
        GenerateValidations(entity);
      }

      End();
    }
    #endregion

    #region Generate Type Definition
    private void GenerateType(Entity model, bool isContext) {
      WriteLine(0, "// Type Definition");
      WriteLine(0, "export type {0} = {", model.Name);

      foreach (Member member in model.Members)
        if (member is X10DerivedAttribute) {
          // Do not generate derived members
        } else if (member.IsNonOwnedAssociation) {
          WriteLine(1, "+{0}: {1},", member.Name, GetType(member));
        } else
          WriteLine(1, "+{0}: {1},", member.Name, GetType(member));

      WriteLine(0, "};");
      WriteLine();
      WriteLine();
    }
    #endregion

    #region Generate Default Entity
    private void GenerateDefaultEntity(Entity model) {
      WriteLine(0, "// Create Default Function");
      WriteLine(0, "export function {0}(): {1} {", ReactCodeGenerator.CreateDefaultFuncName(model), model.Name);
      WriteLine(1, "return {");

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
          if (association.Owns) {
            Entity entity = association.ReferencedEntity;
            string funcName = "createDefault" + entity.Name;
            importsPlaceholder.Import(funcName, entity);
            return funcName + "()";
          } else
            return "null";
        }
      } else if (member is X10RegularAttribute attribute) {
        if (attribute.IsId) {
          ImportsPlaceholder.Import("v4 as uuid", "uuid", ImportLevel.ThirdParty);
          return "uuid()";
        }

        object defaultValue = attribute.DefaultValue;
        DataType dataType = attribute.DataType;

        if (defaultValue == null) {
          if (dataType == DataTypes.Singleton.Boolean) return "false";
          if (dataType == DataTypes.Singleton.String) return "''";
        } else
          return TypedLiteralToString(defaultValue, dataType as DataTypeEnum, false);
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
        PushSourceVariableName(VariableName(entity));
        ExpBase expression = attribute.Expression;

        // Method signature
        WriteLine(0, "export function {0}({1}: {",
          DerivedAttrFuncName(attribute),
          SourceVariableName);

        foreach (X10RegularAttribute regular in FormulaUtils.ExtractSourceRegularAttributes(expression))
          if (!regular.Owner.IsContext)
            WriteLine(1, "+{0}: {1},", regular.Name, GetType(regular));

        WriteLine(0, "}): {0} {", GetType(attribute));

        // Method Body
        WriteAppContextIfNeeded(new ExpBase[] { expression });
        WriteLine(1, "const result = {0};", ExpressionToString(expression));

        if (IsNumeric(attribute.DataType))
          WriteLine(1, "return isNaN(result) ? null : result;");
        else
          WriteLine(1, "return result;");

        // Method termination
        WriteLine(0, "}");
        WriteLine();

        PopSourceVariableName();
      }

      WriteLine();
      WriteLine();
    }
    #endregion

    #region Generate Validations
    private void GenerateValidations(Entity entity) {
      WriteLine(0, "// Validations");

      string varName = VariableName(entity);
      string entityName = entity.Name;

      ImportsPlaceholder.ImportType("FormError", "react_lib/form/FormProvider", ImportLevel.ThirdParty);
      WriteLine(0, "export function {0}({1}: {2}, prefix?: string): $ReadOnlyArray<FormError> { ",
        CalculateErrorsFuncName(entity),
        varName,
        entityName);

      WriteAppContextIfNeeded(entity.Validations.Select(x => x.TriggerExpression));
      WriteLine(1, "const errors = [];");
      WriteLine();

      GenerateValidationsMandatory(entity);
      GenerateValidationsOwnedAssociations(entity);
      GenerateValidationsExplicit(entity);

      WriteLine();
      WriteLine(1, "return errors;");
      WriteLine(0, "}");
      WriteLine();
    }

    private void GenerateValidationsMandatory(Entity entity) {
      foreach (Member member in entity.Members) {
        string varName = VariableName(entity);
        string humanName = NameUtils.CamelCaseToHumanReadable(member.Name);
        bool canBeEmpty = CanBeEmpty(member);

        ImportsPlaceholder.ImportFunction(HelperFunctions.IsBlank);
        ImportsPlaceholder.Import("addError", "react_lib/form/FormProvider", ImportLevel.ThirdParty);

        if (!member.IsReadOnly && canBeEmpty && member.IsMandatory) {
          WriteLine(1, "if (isBlank({0}.{1}))", varName, member.Name);
          WriteLine(2, "addError(errors, prefix, '{0} is required', ['{1}']);", humanName, member.Name);
        }
      }
    }

    private bool CanBeEmpty(Member member) {
      if (member is X10Attribute attr) {
        // List the data-types that can never be empty on a UI
        DataType dataType = attr.DataType;
        if (dataType == DataTypes.Singleton.Boolean) return false;
        return true;
      } else if (member is Association association) {
        return !association.Owns;
      } else
        throw new NotImplementedException("Unknown member type");
    }

    private void GenerateValidationsOwnedAssociations(Entity entity) {
      IEnumerable<Association> ownedAssociations = entity.Associations.Where(x => x.Owns && !x.IsMany);
      if (ownedAssociations.Any()) {
        WriteLine();
        foreach (Association association in ownedAssociations) {
          string varName = VariableName(entity);
          bool applicableWhen = GenerateApplicableWhen(association, varName);
          WriteLine(1 + (applicableWhen ? 1 : 0), "errors.push(...{0}({1}.{2}, '{2}'));", 
            CalculateErrorsFuncName(association.ReferencedEntity),
            varName,
            association.Name);
          ImportsPlaceholder.ImportCalculateErrorsFunc(association.ReferencedEntity);
        }
      }
    }

    private bool GenerateApplicableWhen(Member member, string varName) {
      string whenApplicable = WhenApplicableFuncName(member);
      if (whenApplicable != null) {
        WriteLine(1, "if ({0}({1}))", whenApplicable, varName);
        return true;
      }
      return false;
    }

    private void GenerateValidationsExplicit(Entity entity) {
      if (entity.Validations.Any()) {
        PushSourceVariableName(VariableName(entity));
        WriteLine();

        foreach (Validation validation in entity.Validations) {

          ExpBase expression = validation.TriggerExpression;
          IEnumerable<string> memberNames = FormulaUtils.ListAll(expression)
            .OfType<ExpIdentifier>()
            .Where(x => x.DataType?.Member?.Owner == entity)
            .Select(x => x.DataType.Member.Name);


          if (memberNames.Count() == 0) {
            Messages.AddError(null, "Validation message has no local member references: " + validation.Trigger);
            continue;
          }

          WriteLine(1, "if ({0})", ExpressionToString(expression));

          WriteLine(2, "addError(errors, prefix, '{0}', {1});", validation.Message, JS.ToArray(memberNames));
        }

        PopSourceVariableName();
      }
    }

    #endregion

    #region Utilities

    private void WriteAppContextIfNeeded(IEnumerable<ExpBase> expressions) {
      if (expressions.Any(x => x.UsesContext)) {
        WriteLine(1, "const {0} = React.useContext(AppContext);", JavaScriptFormulaWriter.CONTEXT_VARIABLE);
        ImportsPlaceholder.ImportAppContext();
      }
    }

    private bool IsNumeric(DataType dataType) {
      return
        dataType == DataTypes.Singleton.Integer ||
        dataType == DataTypes.Singleton.Float;
    }

    private string GetType(Member member) {
      if (member is Association association) {
        Entity refedEntity = association.ReferencedEntity;

        if (association.Owns) {
          ImportsPlaceholder.ImportType(refedEntity);

          if (association.IsMany)
            return string.Format("$ReadOnlyArray<{0}>", refedEntity.Name);
          else
            // Generate mandatory even if not mandatory. We ensure that non-mandatory entities
            // are filled with default values when processing the GraphQL results. This ensures
            // that we have default data if the users tarts to edit such entities which previously
            // have been hidden.
            return refedEntity.Name;
        } else {
          if (association.IsMany)
            return "$ReadOnlyArray<string>";
          // Always generate optional - even if mandatory, will not be filled in initially
          return "?string";   // Relay ID
        }
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
      if (dataType == DataTypes.Singleton.Timestamp) return "string";
      if (dataType == DataTypes.Singleton.Float) return "number";
      if (dataType == DataTypes.Singleton.Integer) return "number";
      if (dataType == DataTypes.Singleton.String) return "string";
      if (dataType == DataTypes.Singleton.Money) return "number";
      if (dataType is DataTypeEnum enumType) return EnumToName(enumType);

      throw new NotImplementedException("Unknown data type: " + dataType.Name);
    }

    internal static bool IsDateType(DataType dataType) {
      return
        dataType == DataTypes.Singleton.Date ||
        dataType == DataTypes.Singleton.Timestamp;
    }
    #endregion
  }
}

using System;
using System.Collections.Generic;
using System.Linq;

using FileInfo = x10.parsing.FileInfo;
using x10.model.definition;
using x10.model.metadata;
using x10.formula;
using x10.utils;
using x10.gen.typescript.placeholder;

namespace x10.gen.typescript.generate {
  public partial class TypeScriptCodeGenerator : CodeGenerator {

    #region Top Level
    public override void GenerateCommon() { }

    public override void Generate(Entity entity) {
      FileInfo fileInfo = entity.TreeElement.FileInfo;
      bool isContext = entity.IsContext;

      Begin(fileInfo, ".ts");

      InsertImportsPlaceholder();

      GenerateType(entity, isContext);
      GenerateEnums(entity);
      GenerateDerivedAttributes(entity);

      if (!isContext) {
        GenerateDefaultFunction(entity);
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
        } else
          WriteLine(1, "readonly {0}{1}: {2},", 
            member.Name, 
            member.IsReadOnly ? "" : "?",
            GetType(member, false));

      WriteLine(0, "};");
      WriteLine();
      WriteLine();
    }
    #endregion

    #region Generate Enums
    private void GenerateEnums(Entity entity) {
      IEnumerable<DataTypeEnum> enums = FindLocalEnums(entity);
      if (enums.Count() == 0)
        return;

      WriteLine(0, "// Enums");

      foreach (DataTypeEnum theEnum in enums)
        GenerateEnum(theEnum);

      WriteLine();
      WriteLine();
    }
    #endregion

    #region Generate Derived Attributes
    private void GenerateDerivedAttributes(Entity entity) {
      if (!entity.DerivedAttributes.Any())
        return;

      WriteLine(0, "// Derived Attribute Functions");

      foreach (X10DerivedAttribute attribute in entity.DerivedAttributes) {
        PushSourceVariableName(VariableName(entity));
        ExpBase expression = attribute.Expression;

        // Method signature
        WriteLine(0, "export function {0}({1}?: {",
          DerivedAttrFuncName(attribute),
          SourceVariableName);

        foreach (X10RegularAttribute regular in FormulaUtils.ExtractSourceRegularAttributes(expression))
          if (!(regular.Owner.IsContext || regular.Owner.IsNonFetchable))
            WriteLine(2, "{0}?: {1},", regular.Name, GetType(regular, false));

        WriteLine(0, "} | null | undefined): {0} | undefined {", GetType(attribute, true));

        // Method Body
        WriteLine(1, "if ({0} == null) return {1};",
          SourceVariableName,
          DefaultEmpty(attribute.DataType));
        
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

    #region Generate Default Function
    private void GenerateDefaultFunction(Entity model) {
      WriteLine(0, "// Create Default Function");
      WriteLine(0, "export function {0}(): {1} {", TypeScriptCodeGenerator.CreateDefaultFuncName(model), model.Name);
      WriteLine(1, "return {");

      foreach (Member member in model.Members)
        if (member is X10DerivedAttribute) {
          // Do not generate derived members
        } else {
          string defaultValue = GetDefaultValue(member, ImportsPlaceholder);
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
          } 
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
        } else if (dataType is DataTypeEnum theEnum) {
          ImportsPlaceholder.ImportGraphqlTypeEnum(theEnum);
          return ToEnumValue(theEnum, defaultValue);
        } else
          return TypedLiteralToString(defaultValue, null, false);
      }

      return "undefined";
    }
    #endregion

    // TODO: Leaving this code as-is in order to avoid trying to "boil the ocean" during the 
    // flow => typescript transition. However, the correct way to do this is to write
    // individual validations, to allow for partial update forms, not just create forms.
    // Also, we should not use React.useContext() in a non-FC method, even though this seems to work.
    #region Generate Validations
    private void GenerateValidations(Entity entity) {
      WriteLine(0, "// Validations");

      string varName = VariableName(entity);
      string entityName = entity.Name;

      ImportsPlaceholder.ImportTypeFromReactLib("FormError", "form/FormProvider");
      WriteLine(0, "export function {0}({1}?: {2}, prefix?: string, inListIndex?: number): FormError[] { ",
        CalculateErrorsFuncName(entity),
        varName,
        entityName);

      WriteAppContextIfNeeded(entity.Validations.Select(x => x.TriggerExpression));
      WriteLine(1, "const errors: FormError[] = [];");
      WriteLine(1, "if ({0} == null ) return errors;", varName);
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
        ImportsPlaceholder.ImportFromReactLib("addError", "form/FormProvider");

        if (!member.IsReadOnly && canBeEmpty && member.IsMandatory) {
          WriteLine(1, "if (isBlank({0}.{1}))", varName, member.Name);
          WriteLine(2, "addError(errors, '{0} is required', ['{1}'], prefix, inListIndex);", humanName, member.Name);
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
      IEnumerable<Association> ownedAssociations = entity.Associations.Where(x => x.Owns);
      if (ownedAssociations.Any()) {
        WriteLine();
        foreach (Association association in ownedAssociations) {
          string varName = VariableName(entity);
          bool applicableWhen = GenerateApplicableWhen(association, varName);

          if (association.IsMany) {
            // parent.association?.forEach((x, ii) => errors.push(...entityCalculateErrors(x, 'association', ii)));
            WriteLine(1, "{0}.{1}?.forEach((x, ii) => errors.push(...{2}(x, '{1}', ii)));", 
              varName,
              association.Name,
              CalculateErrorsFuncName(association.ReferencedEntity));
          } else {
            WriteLine(1 + (applicableWhen ? 1 : 0), "errors.push(...{0}({1}.{2}, '{2}'));", 
              CalculateErrorsFuncName(association.ReferencedEntity),
              varName,
              association.Name);
          }
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

          WriteLine(2, "addError(errors, '{0}', {1}, prefix, inListIndex);", validation.Message, JS.ToArray(memberNames));
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

    private string GetType(Member member, bool forceOptional) {
      if (member is Association association) {
        Entity refedEntity = association.ReferencedEntity;

        ImportsPlaceholder.ImportType(refedEntity);

        if (association.IsMany)
          return string.Format("{0}[]", refedEntity.Name);
        else {
            // Generate mandatory even if not mandatory. We ensure that non-mandatory entities
            // are filled with default values when processing the GraphQL results. This ensures
            // that we have default data if the users starts to edit such entities which previously
            // have been hidden.
            return refedEntity.Name;
        }
      } else if (member is X10Attribute attribute) {
        bool optional = !IsMandatory(attribute) || forceOptional;
        if (IsNeverOptional(attribute.DataType))
          optional = false;

        return GetAtomicType(member.Owner, attribute.DataType) + 
          (optional ? " | null | undefined" : "");
        
      } else
        throw new NotImplementedException("Unknown member type: " + member.GetType());
    }

    private bool IsNeverOptional(DataType dataType) {
      // This mandatory/optional decision is made based on the usual UI used to represent
      // the data type. It is assumed that booleans are represented by CheckBoxes and
      // Strings are represented by TextInput's or similar where the empty string represents
      // no user input
      return 
        dataType == DataTypes.Singleton.Boolean ||
        dataType == DataTypes.Singleton.String;
    }

    private string DefaultEmpty(DataType dataType) {
        if (dataType == DataTypes.Singleton.Boolean) return "false";
        if (dataType == DataTypes.Singleton.String) return "''";
        return "null";
    }

    private bool IsMandatory(X10Attribute attribute) {
      // If an attribute is read-only, editing is not an issue, so simply reflect the
      // "mandatory-ness" of the attribute
      if (attribute.IsId)
        return attribute.IsMandatory;

      DataType dataType = attribute.DataType;

      if (IsNeverOptional(dataType))
        return true;

      // All UI element used to represent these types can have a null state. For example,
      // a DateInput UI can have a state where no value has been specified, and so for all
      // the other types.
      if (dataType == DataTypes.Singleton.Date) return false;
      if (dataType == DataTypes.Singleton.Time) return false;
      if (dataType == DataTypes.Singleton.Float) return false;
      if (dataType == DataTypes.Singleton.Integer) return false;
      if (dataType == DataTypes.Singleton.Timestamp) return false;
      if (dataType == DataTypes.Singleton.Money) return false;
      if (dataType is DataTypeEnum enumType) return false;

      throw new NotImplementedException("Unknown data type: " + dataType.Name);
    }

    private string GetAtomicType(Entity entity, DataType dataType) {
      if (dataType == DataTypes.Singleton.Boolean) return "boolean";
      if (dataType == DataTypes.Singleton.Date) return "string";
      if (dataType == DataTypes.Singleton.Time) return "string";
      if (dataType == DataTypes.Singleton.Timestamp) return "string";
      if (dataType == DataTypes.Singleton.Float) return "number";
      if (dataType == DataTypes.Singleton.Integer) return "number";
      if (dataType == DataTypes.Singleton.String) return "string";
      if (dataType == DataTypes.Singleton.Money) return "number";
      if (dataType is DataTypeEnum enumType) {
        if (entity.TreeElement.FileInfo.RelativePath != enumType.TreeElement.FileInfo.RelativePath) {
          string enumName = EnumToTypeName(enumType);
          ImportsPlaceholder.ImportType(enumName, enumType);
        }
        return EnumToTypeName(enumType);
      }

      throw new NotImplementedException("Unknown data type: " + dataType.Name);
    }
    #endregion
  }
}

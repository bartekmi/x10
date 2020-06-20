using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using x10.parsing;
using x10.model.metadata;
using x10.model.definition;
using x10.model;
using x10.formula;

namespace x10.compiler {
  public class EntityCompilerPass2 {

    public MessageBucket Messages { get; private set; }
    private readonly AllEntities _allEntities;
    private readonly AllEnums _allEnums;
    private readonly AllFunctions _allFunctions;

    public EntityCompilerPass2(
      MessageBucket messages,
      AllEntities allEntities,
      AllEnums allEnums,
      AllFunctions allFunctions) {

      Messages = messages;
      _allEntities = allEntities;
      _allEnums = allEnums;
      _allFunctions = allFunctions;
    }

    internal void CompileAllEntities() {
      VerifyUniquenessOfAllEntityNames();
      VerifyUniquenessOfAllEnumNames();
      InvokePass2_Actions();
      VerifyUniquenessOfMemberNamesInInheritance();
      ParseAllFormulas();
    }

    private void VerifyUniquenessOfAllEntityNames() {
      UniquenessChecker.Check("name",
        _allEntities.All,
        Messages,
        "The Entity name '{0}' is not unique.");
    }

    private void VerifyUniquenessOfAllEnumNames() {
      UniquenessChecker.Check("name",
        _allEnums.All,
        Messages,
        "The Enum name '{0}' is not unique.");
    }

    private void InvokePass2_Actions() {
      // If any of the ModelAttributeValue's - either for the entity, or
      // for any members - have a Pass-2 action, invoke it.
      // Examples of Pass-2 actions are hydrating 'InheritsFrom' and 'ReferencedEntity'
      foreach (Entity entity in _allEntities.All) {
        foreach (ModelAttributeValue value in entity.AttributeValues.OrderBy(x => x))
          value.Definition.Pass2Action?.Invoke(Messages, _allEntities, _allEnums, entity, value);

        foreach (Member member in entity.LocalMembers)
          foreach (ModelAttributeValue value in member.AttributeValues.OrderBy(x => x)) {
            if ((value.Definition as ModelAttributeDefinitionAtomic)?.DataTypeMustBeSameAsAttribute == true)
              ConvertValueToDataTypeOfAttribute(member as X10Attribute, value);
            value.Definition.Pass2Action?.Invoke(Messages, _allEntities, _allEnums, member, value);
          }
      }
    }

    private void ConvertValueToDataTypeOfAttribute(X10Attribute attr, ModelAttributeValue value) {
      if (attr?.DataType == null || value.Value == null)
        return;

      string stringValue = value.Value.ToString();
      object typedValue = attr.DataType.Parse(stringValue, Messages, value.TreeElement, value.Definition.Name);
      value.Value = typedValue;
      AttributeReader.SetValueViaSetter(value.Definition, attr, typedValue);
    }


    private void VerifyUniquenessOfMemberNamesInInheritance() {
      // Verify Uniqueness of all member names within inheritance hierarchies
      // We can't put this in Pass 2 action of attribute definition
      // because we can't guarantee that the entire inheritance tree has been "hydrated"
      // when Pass 2 above is running
      // However, inheritance hierarchy is full done at this point.
      foreach (Entity entity in _allEntities.All) {
        UniquenessChecker.Check("name",
          entity.Members,
          Messages,
          "The name '{0}' is not unique among all the attributes and association of this Entity (possibly involving the entire inheritance hierarchy).");
      }
    }


    private void ParseAllFormulas() {
      FormulaParser parser = new FormulaParser(Messages, _allEntities, _allEnums, _allFunctions);

      foreach (Entity entity in _allEntities.All) {
        ExpDataType dataType = new ExpDataType(entity);

        foreach (ModelAttributeValue value in entity.AttributeValues)
          CheckIfFormulaAndParse(parser, null, dataType, value);

        foreach (Member member in entity.LocalMembers)
          foreach (ModelAttributeValue value in member.AttributeValues)
            CheckIfFormulaAndParse(parser, member, dataType, value);
      }
    }

    private void CheckIfFormulaAndParse(FormulaParser parser, Member member, ExpDataType modelDataType, ModelAttributeValue value) {
      if (value.Definition is ModelAttributeDefinitionAtomic atomicDef) {
        if (value.Formula != null) {
          value.Expression = parser.Parse(value.TreeElement, value.Formula, modelDataType);
          ValidateReturnedDataType(member, atomicDef, value);
        } else {
          if (atomicDef.MustBeFormula)
            Messages.AddError(value.TreeElement, "Attribute '{0}' must be a formula (must start with '=').", atomicDef.Name);
        }
      }
    }

    private void ValidateReturnedDataType(Member member, ModelAttributeDefinitionAtomic atomicDef, ModelAttributeValue value) {
      ExpDataType returnedDataType = value.Expression.DataType;
      if (returnedDataType.IsError)
        return;

      ExpDataType expectedReturnType = atomicDef.DataTypeMustBeSameAsAttribute ?
        member.GetExpressionDataType() :
        new ExpDataType(atomicDef.DataType);

      // Since anything can be converted to String, don't worry about type checking
      if (expectedReturnType.IsString)
        return;

      if (!returnedDataType.Equals(expectedReturnType))
        Messages.AddError(value.TreeElement, "Expected expression to return {0}, but it returns {1}",
          expectedReturnType, returnedDataType);
    }
  }
}
















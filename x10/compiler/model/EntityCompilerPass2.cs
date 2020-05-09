using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using x10.parsing;
using x10.model.metadata;
using x10.model.definition;
using x10.model;

namespace x10.compiler {
  public class EntityCompilerPass2 {

    public MessageBucket Messages { get; private set; }
    private readonly AllEntities _allEntities;
    private readonly AllEnums _allEnums;

    public EntityCompilerPass2(
      MessageBucket messages,
      AllEntities allEntities,
      AllEnums allEnums) {

      Messages = messages;
      _allEntities = allEntities;
      _allEnums = allEnums;
    }

    internal void CompileAllEntities() {

      // Verify Uniqueness of all Entity names
      UniquenessChecker.Check("name",
        _allEntities.All,
        Messages,
        "The Entity name '{0}' is not unique.");

      // Verify Uniqueness of all Enum names
      UniquenessChecker.Check("name",
        _allEnums.All,
        Messages,
        "The Enum name '{0}' is not unique.");

      // If any of the ModelAttributeValue's - either for the entity, or
      // for any members - have a Pass-2 action, invoke it.
      // Examples of Pass-2 actions are hydrating 'InheritsFrom' and 'ReferencedEntity'
      foreach (Entity entity in _allEntities.All) {
        foreach (ModelAttributeValue value in entity.AttributeValues)
          value.Definition.Pass2Action?.Invoke(Messages, _allEntities, _allEnums, entity, value);

        foreach (Member member in entity.LocalMembers)
          foreach (ModelAttributeValue value in member.AttributeValues) {
            if ((value.Definition as ModelAttributeDefinitionAtomic)?.DataTypeMustBeSameAsAttribute == true)
              ConvertValueToDataTypeOfAttribute(member as X10Attribute, value);
            value.Definition.Pass2Action?.Invoke(Messages, _allEntities, _allEnums, member, value);
          }
      }

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

    private void ConvertValueToDataTypeOfAttribute(X10Attribute attr, ModelAttributeValue value) {
      if (attr?.DataType == null)
        return;

      string stringValue = value.Value.ToString();
      if (AttributeReader.IsFormula(stringValue))
        value.Formula = stringValue;
      else {
        object typedValue = attr.DataType.Parse(stringValue, Messages, value.TreeElement, value.Definition.Name);
        value.Value = typedValue;
        AttributeReader.SetValueViaSetter(value.Definition, attr, typedValue);
      }
    }
  }
}
















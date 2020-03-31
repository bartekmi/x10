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

    public EntityCompilerPass2(MessageBucket messages, AllEntities allEntities) {
      Messages = messages;
      _allEntities = allEntities;
    }

    internal void CompileAllEntities() {

      // Verify Uniqueness of all Entity names
      UniquenessChecker.Check("name",
        _allEntities.All,
        Messages,
        "The Entity name '{0}' is not unique.");

      // Verify Uniqueness of all Enum names
      UniquenessChecker.Check("name",
        DataTypes.Singleton.ModelEnums,
        Messages,
        "The Enum name '{0}' is not unique.");


      // If any of the ModelAttributeValue's - either for the entity, or
      // for any members - have a Pass-2 action, invoke it.
      // Examples of Pass-2 actions are hydrating 'InheritsFrom' and 'ReferencedEntity'
      foreach (Entity entity in _allEntities.All) {
        foreach (ModelAttributeValue value in entity.AttributeValues)
          value.Definition.Pass2Action?.Invoke(Messages, _allEntities, entity, value);

        foreach (Member member in entity.Members)
          foreach (ModelAttributeValue value in member.AttributeValues)
            value.Definition.Pass2Action?.Invoke(Messages, _allEntities, member, value);
      }
    }
  }
}
















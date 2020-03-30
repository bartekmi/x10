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


      foreach (Entity entity in _allEntities.All)
        foreach (ModelAttributeValue value in entity.AttributeValues)
          if (value.Definition.Pass2Action != null)
            value.Definition.Pass2Action(Messages, _allEntities, entity, value);
    }
  }
}
















using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using x10.parsing;
using x10.model.metadata;
using x10.model.definition;
using System.Reflection.Metadata.Ecma335;

namespace x10.compiler {
  public class EntityCompilerPass2 {

    public MessageBucket Messages { get; private set; }
    private IEnumerable<Entity> _entitiesList;
    private Dictionary<string, List<Entity>> _entitiesByName;

    public EntityCompilerPass2(MessageBucket messages, IEnumerable<Entity> entitiesList) {
      Messages = messages;
      _entitiesList = entitiesList;
    }

    internal void CompileAllEntities() {

      // Verify Uniqueness of all Entity names
      UniquenessChecker.Check("name",
        _entitiesList,
        Messages,
        "The Entity name '{0}' is not unique.");

      // Verify Uniqueness of all Enum names
      UniquenessChecker.Check("name",
        DataTypes.Singleton.ModelEnums,
        Messages,
        "The Enum name '{0}' is not unique.");


      // The reason the values are a list is to account for problems where multiple entities with
      // the same name have been defined 
      var entitiesGroupedByName = _entitiesList.GroupBy(x => x.Name);
      _entitiesByName = entitiesGroupedByName.ToDictionary(g => g.Key, g => new List<Entity>(g));

      foreach (Entity entity in _entitiesList) {
        RehydrateAssociationLinks(entity);
        RehydrateInheritsFrom(entity);
      }
    }

    private void RehydrateAssociationLinks(Entity entity) {
      foreach (Association association in entity.Associations) {
        if (association.ReferencedEntityName != null)
          association.ReferencedEntity = FindEntityByNameWithError(association.ReferencedEntityName,
            association,
            "dataType");
      }
    }

    private void RehydrateInheritsFrom(Entity entity) {
      if (entity.InheritsFromName == null)
        return;

      entity.InheritsFrom = FindEntityByNameWithError(entity.InheritsFromName,
        entity,
        "inheritsFrom");
    }

    private Entity FindEntityByNameWithError(string entityName, IAcceptsModelAttributeValues modelComponent, string attributeName) {
      ModelAttributeValue attributeValue = AttributeUtils.FindAttribute(modelComponent, attributeName);

      // Check if entity exists
      if (!_entitiesByName.TryGetValue(entityName, out List<Entity> entities)) {
        Messages.AddError(attributeValue.TreeElement,
          string.Format("Entity '{0}' not found", entityName));
        return null;
      }

      if (entities.Count > 1) {
        Messages.AddError(attributeValue.TreeElement,
          string.Format("Multiple entities with the name '{0}' exist", entityName));
        return null;
      }

      return entities.Single();
    }
  }
}
















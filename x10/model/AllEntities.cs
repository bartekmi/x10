using System;
using System.Collections.Generic;
using System.Linq;

using x10.model.definition;
using x10.parsing;

namespace x10.model {
  public class AllEntities {
    private readonly Dictionary<string, List<Entity>> _entitiesByName;
    private readonly MessageBucket _messages;

    public AllEntities(IEnumerable<Entity> entities, MessageBucket messages) {
      // The reason the values are a list is to account for problems where multiple entities with
      // the same name have been defined 
      var entitiesGroupedByName = entities.GroupBy(x => x.Name);
      _entitiesByName = entitiesGroupedByName.ToDictionary(g => g.Key, g => new List<Entity>(g));

      _messages = messages;
    }

    public IEnumerable<Entity> All { 
      get { return _entitiesByName.Values.SelectMany(x => x);  } 
    }

    public Entity FindEntityByName(string entityName) {
      return _entitiesByName[entityName].Single();
    }

    public Entity FindEntityByNameWithError(string entityName, IParseElement parseElement) {
      // Check if entity exists
      if (!_entitiesByName.TryGetValue(entityName, out List<Entity> entities)) {
        _messages.AddError(parseElement,
          string.Format("Entity '{0}' not found", entityName));
        return null;
      }

      if (entities.Count > 1) {
        _messages.AddError(parseElement,
          string.Format("Multiple entities with the name '{0}' exist", entityName));
        return null;
      }

      return entities.Single();
    }
  }
}

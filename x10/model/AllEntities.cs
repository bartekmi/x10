using System;
using System.Collections.Generic;
using System.Linq;

using x10.model.definition;
using x10.parsing;

namespace x10.model {
  public class AllEntities {
    private readonly MessageBucket _messages;
    private readonly Dictionary<string, List<Entity>> _entitiesByName;
    
    public AllEntities(MessageBucket messages, IEnumerable<Entity> entities, Entity contextEntity = null) {
      _messages = messages;

      // The reason the values are a list is to account for problems where multiple entities with
      // the same name have been defined 
      var entitiesGroupedByName = entities.GroupBy(x => x.Name);
      _entitiesByName = entitiesGroupedByName.ToDictionary(g => g.Key, g => new List<Entity>(g));
    }

    public IEnumerable<Entity> All { 
      get { return _entitiesByName.Values.SelectMany(x => x);  } 
    }

    public Entity FindEntityByName(string entityName) {
      if (_entitiesByName.TryGetValue(entityName, out List<Entity> entities)) 
        return entities.FirstOrDefault();
      return null;
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

    public Member FindMemberByPath(string path) {
      string[] components = path.Split(".");

      Entity entity = FindEntityByName(components.First());
      if (entity == null)
        throw new Exception("Entity does not exist: " + entity);
      
      Member member = null;
      foreach (string memberName in components.Skip(1)) {
        member = entity.FindMemberByName(memberName);
        if (member == null)
          throw new Exception(string.Format("Member does not exist: {0}.{1}", entity, memberName));

        if (member is Association assoc)
          entity = assoc.ReferencedEntity;
        // Skipping check... If member is NOT association, we better be at end of path
      }

      return member;
    }

    internal Entity FindContextEntityWithError(IParseElement parseElement) {
      Entity contextEntity = FindEntityByNameWithError(ModelValidationUtils.CONTEXT_ENTITY_NAME, parseElement);
      if (contextEntity == null)
        _messages.AddError(parseElement,
          "In order to use Context-level data (a typical example would be the currently logged-in User), " +
          "you must define an Entity called {0}", ModelValidationUtils.CONTEXT_ENTITY_NAME);
      return contextEntity;
    }
  }
}

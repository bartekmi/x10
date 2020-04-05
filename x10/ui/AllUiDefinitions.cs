using System;
using System.Collections.Generic;
using System.Linq;

using x10.parsing;
using x10.ui.metadata;

namespace x10.model {
  public class AllUiDefinitions {
    private readonly Dictionary<string, List<UiDefinition>> _definitionsByName;
    private readonly MessageBucket _messages;

    public AllUiDefinitions(IEnumerable<UiDefinition> definitions, MessageBucket messages) {
      // The reason the values are a list is to account for problems where multiple entities with
      // the same name have been defined 
      var entitiesGroupedByName = definitions.GroupBy(x => x.Name);
      _definitionsByName = entitiesGroupedByName.ToDictionary(g => g.Key, g => new List<UiDefinition>(g));

      _messages = messages;
    }

    public IEnumerable<UiDefinition> All { 
      get { return _definitionsByName.Values.SelectMany(x => x);  } 
    }

    public UiDefinition FindDefinitionByNameWithError(string entityName, IParseElement parseElement) {
      // Check if entity exists
      if (!_definitionsByName.TryGetValue(entityName, out List<UiDefinition> definitions)) {
        _messages.AddError(parseElement,
          string.Format("Entity '{0}' not found", entityName));
        return null;
      }

      if (definitions.Count > 1) {
        _messages.AddError(parseElement,
          string.Format("Multiple entities with the name '{0}' exist", entityName));
        return null;
      }

      return definitions.Single();
    }
  }
}

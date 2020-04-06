using System;
using System.Collections.Generic;
using System.Linq;

using x10.parsing;
using x10.ui.composition;
using x10.ui.metadata;

namespace x10.compiler {
  public class AllUiDefinitions {
    private readonly Dictionary<string, List<UiDefinitionX10>> _uiDefinitionsByName;
    private readonly MessageBucket _messages;
    private readonly UiLibrary[] _libraries;

    public AllUiDefinitions(MessageBucket messages, IEnumerable<UiDefinitionX10> components, params UiLibrary[] libraries) {
      // The reason the values are a list is to account for problems where multiple UI components with
      // the same name have been defined 
      var componentsGroupedByName = components.GroupBy(x => x.Name);
      _uiDefinitionsByName = componentsGroupedByName.ToDictionary(g => g.Key, g => new List<UiDefinitionX10>(g));

      _messages = messages;
      _libraries = libraries;
    }

    public IEnumerable<UiDefinitionX10> All {
      get { return _uiDefinitionsByName.Values.SelectMany(x => x); }
    }

    public UiDefinition FindDefinitionByNameWithError(string componentName, IParseElement parseElement) {
      // FUTURE: At some point, we'll have to worry about name spaces and name collisions. We are not
      // there yet.

      foreach (UiLibrary library in _libraries) {
        UiDefinition definition = library.FindComponentByName(componentName);
        if (definition != null)
          return definition;
      }

      // Check if component exists
      if (!_uiDefinitionsByName.TryGetValue(componentName, out List<UiDefinitionX10> definitions)) {
        _messages.AddError(parseElement,
          string.Format("UI Component '{0}' not found", componentName));
        return null;
      }

      if (definitions.Count > 1) {
        _messages.AddError(parseElement,
          string.Format("Multiple UI Components with the name '{0}' exist", componentName));
        return null;
      }

      return definitions.Single();
    }

    internal void UiComponentUniquenessCheck() {
      foreach (var definitions in _uiDefinitionsByName.Where(x => x.Value.Count() > 1)) {
        foreach (UiDefinitionX10 definition in definitions.Value) {
          UiAttributeValue attribute = UiAttributeUtils.FindAttribute(definition, UiAttributeDefinitions.ELEMENT_NAME);
          _messages.AddError(attribute.XmlBase,
            String.Format("The UI Component name '{0}' is not unique.", definitions.Key));
        }

      }
    }
  }
}

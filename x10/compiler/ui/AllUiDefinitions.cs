using System;
using System.Collections.Generic;
using System.Linq;

using x10.parsing;
using x10.model.definition;
using x10.ui.composition;
using x10.ui.metadata;

namespace x10.compiler {
  public class AllUiDefinitions {
    private readonly Dictionary<string, List<ClassDefX10>> _uiDefinitionsByName;
    private readonly MessageBucket _messages;
    private readonly IEnumerable<UiLibrary> _libraries;

    public AllUiDefinitions(MessageBucket messages, IEnumerable<ClassDefX10> components, IEnumerable<UiLibrary> libraries) {
      // The reason the values are a list is to account for problems where multiple UI components with
      // the same name have been defined 
      if (components == null)
        components = new ClassDefX10[0];

      var componentsGroupedByName = components.Where(x => x != null).GroupBy(x => x.Name);
      _uiDefinitionsByName = componentsGroupedByName.ToDictionary(g => g.Key, g => new List<ClassDefX10>(g));

      _messages = messages;
      _libraries = libraries;
    }

    public IEnumerable<ClassDefX10> All {
      get { return _uiDefinitionsByName.Values.SelectMany(x => x); }
    }

    public ClassDef FindDefinitionByNameWithError(string componentName, IParseElement parseElement) {
      // FUTURE: At some point, we'll have to worry about name spaces and name collisions. We are not
      // there yet.

      foreach (UiLibrary library in _libraries) {
        ClassDef definition = library.FindComponentByName(componentName);
        if (definition != null)
          return definition;
      }

      // Check if component exists
      if (!_uiDefinitionsByName.TryGetValue(componentName, out List<ClassDefX10> definitions)) {
        IEnumerable<string> allValidNames = _libraries.SelectMany(x => x.AllNames).Concat(_uiDefinitionsByName.Keys);
        _messages.AddErrorDidYouMean(parseElement, componentName, allValidNames, "UI Component '{0}' not found", componentName);
        return null;
      }

      if (definitions.Count > 1) {
        _messages.AddError(parseElement,
          string.Format("Multiple UI Components with the name '{0}' exist", componentName));
        return null;
      }

      return definitions.Single();
    }

    public bool IsWrapper(ClassDef classDef) {
      return _libraries.Any(x => x.IsWrapper(classDef));
    }

    internal void UiComponentUniquenessCheck() {
      foreach (var definitions in _uiDefinitionsByName.Where(x => x.Value.Count() > 1)) {
        foreach (ClassDefX10 definition in definitions.Value) {
          UiAttributeValue attribute = definition.FindAttributeValue(ParserXml.ELEMENT_NAME);
          _messages.AddError(attribute.XmlBase,
            String.Format("The UI Component name '{0}' is not unique.", definitions.Key));
        }
      }
    }

    internal ClassDef FindUiComponentForDataType(X10Attribute attribute, IParseElement parseElement) {
      foreach (UiLibrary library in _libraries) {
        ClassDef uiComponent = library.FindUiComponentForDataType(attribute);
        if (uiComponent != null)
          return uiComponent;
      }

      _messages.AddError(parseElement, "DataType {0} does not have an associated UI Component.", attribute.DataType.Name);
      return null;
    }
  }
}

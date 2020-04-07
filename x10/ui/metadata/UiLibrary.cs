using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace x10.ui.metadata {
  public class UiLibrary {
    public string Name { get; set; }
    public string Description { get; set; }

    private Dictionary<string, UiDefinition> _definitionsByName;

    public UiLibrary(IEnumerable<UiDefinition> definitions) {
      _definitionsByName = definitions.ToDictionary(x => x.Name);
    }

    public UiDefinition FindComponentByName(string componentName) {
      _definitionsByName.TryGetValue(componentName, out UiDefinition definition);
      return definition;
    }

    public override string ToString() {
      return "UI Library: " + Name;
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace x10.ui.metadata {
  public class UiLibrary {
    public string Name { get; set; }
    public string Description { get; set; }

    private Dictionary<string, ClassDef> _definitionsByName;

    public UiLibrary(IEnumerable<ClassDef> definitions) {
      _definitionsByName = definitions.ToDictionary(x => x.Name);
    }

    public ClassDef FindComponentByName(string componentName) {
      _definitionsByName.TryGetValue(componentName, out ClassDef definition);
      return definition;
    }

    public override string ToString() {
      return "UI Library: " + Name;
    }
  }
}

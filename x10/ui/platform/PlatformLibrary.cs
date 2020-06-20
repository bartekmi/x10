using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using x10.ui.metadata;

namespace x10.ui.platform {
  public class PlatformLibrary {
    public string ImportPath { get; set; }

    private readonly UiLibrary LogicalLibrary;
    private readonly Dictionary<string, PlatformClassDef> _definitionsByLogicalName;

    // Derived
    public IEnumerable<PlatformClassDef> All { get { return _definitionsByLogicalName.Values; } }

    public PlatformLibrary(UiLibrary logicalLibrary, IEnumerable<PlatformClassDef> definitions) {
      LogicalLibrary = logicalLibrary;
      _definitionsByLogicalName = definitions.ToDictionary(x => x.LogicalName);
    }

    #region Utility Methods
    public PlatformClassDef FindComponentByLogicalName(string logicalName) {
      _definitionsByLogicalName.TryGetValue(logicalName, out PlatformClassDef definition);
      return definition;
    }
    #endregion

  }
}

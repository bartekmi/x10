using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using x10.ui.metadata;
using x10.parsing;

namespace x10.ui.platform {
  public class PlatformLibrary {
    public string Name { get; set; }
    public string Description { get; set; }

    // Use this path if the PlatformClassDef does not specify a particular import path
    public string DefaultImportPath { get; set; }

    // If present, pre-pend this to the specific import path declared by any PlatformClassDef
    // Useful for generating code where the helper library (e.g. react_lib) is mounted in a custom location
    public string NonDefaultImportPath { get; set; }

    public UiLibrary LogicalLibrary { get; private set; }
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

    public bool HydrateAndValidate(MessageBucket messages) {
      PlatformLibraryValidator validator = new PlatformLibraryValidator(messages);

      int messageCountBefore = messages.Messages.Count;
      validator.HydrateAndValidate(LogicalLibrary, this);
      int messageCountAfter = messages.Messages.Count;

      return messageCountBefore == messageCountAfter;
    }

    #endregion

  }
}

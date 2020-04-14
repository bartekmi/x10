using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using x10.model.metadata;
using x10.parsing;

namespace x10.ui.metadata {
  public class UiLibrary {
    public string Name { get; set; }
    public string Description { get; set; }

    private readonly Dictionary<string, ClassDef> _definitionsByName;
    private readonly Dictionary<DataType, ClassDef> _dataTypesToComponent;

    // Derived
    public IEnumerable<ClassDef> All { get { return _definitionsByName.Values; } }

    // Constructor
    public UiLibrary(IEnumerable<ClassDef> definitions) {
      _definitionsByName = definitions.ToDictionary(x => x.Name);
      _dataTypesToComponent = new Dictionary<DataType, ClassDef>();
    }

    public ClassDef FindComponentByName(string componentName) {
      _definitionsByName.TryGetValue(componentName, out ClassDef definition);
      return definition;
    }

    public void AddDataTypeToComponentAssociation(DataType dataType, string componentName) {
      ClassDef uiComponent = FindComponentByName(componentName);
      if (uiComponent == null)
        throw new Exception(string.Format("Attempting to set default component for data type {0}. Component {1} does not exist",
          dataType.Name, componentName));

      _dataTypesToComponent[dataType] = uiComponent;
    }

    public ClassDef FindUiComponentForDataType(DataType dataType) {
      _dataTypesToComponent.TryGetValue(dataType, out ClassDef uiComponent);
      return uiComponent;
    }

    public bool HydrateAndValidate(MessageBucket messages) {
      UiLibraryValidator validator = new UiLibraryValidator(messages);

      int messageCountBefore = messages.Messages.Count;
      validator.HydrateAndValidate(this);
      int messageCountAfter = messages.Messages.Count;

      return messageCountBefore == messageCountAfter;
    }

    public override string ToString() {
      return "UI Library: " + Name;
    }
  }
}

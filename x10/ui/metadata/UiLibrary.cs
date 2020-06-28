using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using x10.parsing;
using x10.model.metadata;
using x10.model.definition;
using System.IO;

namespace x10.ui.metadata {
  public class UiLibrary {
    #region Properties
    public string Name { get; set; }
    public string Description { get; set; }

    private readonly Dictionary<string, ClassDef> _definitionsByName;
    private readonly Dictionary<DataType, ClassDef> _dataTypesToComponent;
    private HashSet<ClassDef> _wrapperComponents;
    private ClassDef _defaultComponentForEnums;

    // Derived
    public IEnumerable<ClassDef> All { get { return _definitionsByName.Values; } }
    #endregion

    #region Creating the Library
    // Constructor
    public UiLibrary(IEnumerable<ClassDef> definitions) {
      _definitionsByName = definitions.ToDictionary(x => x.Name);
      _dataTypesToComponent = new Dictionary<DataType, ClassDef>();
    }

    // TODO: This may likely need other fields in the future, in particular: readOnly and isMandatory,
    // as this may effect the type of component we want to use.
    public void AddDataTypeToComponentAssociation(DataType dataType, string componentName) {
      ClassDef uiComponent = FindComponentByName(componentName);
      if (uiComponent == null)
        throw new Exception(string.Format("Attempting to set default component for data type {0}. Component {1} does not exist",
          dataType.Name, componentName));

      _dataTypesToComponent[dataType] = uiComponent;
    }

    // TODO: Ditto here
    public void SetComponentForEnums(string componentName) {
      ClassDef uiComponent = FindComponentByName(componentName);
      if (uiComponent == null)
        throw new Exception(string.Format("Attempting to set default component for enums. Component {0} does not exist",
          componentName));

      _defaultComponentForEnums = uiComponent;
    }

    public bool HydrateAndValidate(MessageBucket messages) {
      UiLibraryValidator validator = new UiLibraryValidator(messages);

      int messageCountBefore = messages.Messages.Count;
      validator.HydrateAndValidate(this);
      int messageCountAfter = messages.Messages.Count;

      _wrapperComponents = new HashSet<ClassDef>(All
        .SelectMany(x => x.ComplexAttributeDefinitions)
        .Select(x => x.ModelRefWrapperComponent)
        .Where(x => x != null));

      return messageCountBefore == messageCountAfter;
    }
    #endregion

    #region Utility Methods
    public ClassDef FindComponentByName(string componentName) {
      _definitionsByName.TryGetValue(componentName, out ClassDef definition);
      return definition;
    }

    public ClassDef FindUiComponentForDataType(X10Attribute attribute) {
      _dataTypesToComponent.TryGetValue(attribute.DataType, out ClassDef uiComponent);

      if (uiComponent == null && attribute.DataType is DataTypeEnum)
        uiComponent = _defaultComponentForEnums;

      return uiComponent;
    }

    public bool IsWrapper(ClassDef classDef) {
      return _wrapperComponents.Contains(classDef);
    }
    #endregion

    #region Documentation Generation
    public void GenerateMarkdown(TextWriter writer) {
      foreach (ClassDef classDef in _definitionsByName.Values.OrderBy(x => x.Name))
        GenerateMarkdownForClassDef(writer, classDef);
    }

    private void GenerateMarkdownForClassDef(TextWriter writer, ClassDef classDef) {
      writer.WriteLine("# Class Definition - '{0}'", classDef.Name);
      writer.WriteLine();
      writer.WriteLine(classDef.Description);
      writer.WriteLine();

      Write(writer, "Inherits From", classDef.InheritsFromName);
      Write(writer, "Expects Data Type", classDef.AtomicDataModel?.Name);
      Write(writer, "Expects Array of Data", classDef?.IsMany == true ? "Yes" : null);

      // Attributes
      writer.WriteLine();

      UiAttributeDefinition primaryAttrDef = classDef.AttributeDefinitions.SingleOrDefault(x => x.IsPrimary);
      if (primaryAttrDef != null)
        GenerateMarkdownForAttribute(writer, primaryAttrDef);

      foreach (UiAttributeDefinition attrDef in classDef.AttributeDefinitions.OrderBy(x => x.Name))
        if (!attrDef.IsPrimary)
          GenerateMarkdownForAttribute(writer, attrDef);

      writer.WriteLine();

    }

    private void GenerateMarkdownForAttribute(TextWriter writer, UiAttributeDefinition attrDef) {
      writer.WriteLine("### Attribute '{0}'{1}", attrDef.Name, 
        attrDef.IsPrimary ? " (Primary)": null);
      writer.WriteLine();
      writer.WriteLine(attrDef.Description);
      writer.WriteLine();

      Write(writer, "Primary Attribute", attrDef.IsPrimary ? "Yes" : null);
      Write(writer, "Mandatory", attrDef.IsMandatory ? "Yes" : null);
      Write(writer, "Expects Array of Values", attrDef.IsMany ? "Yes" : null);

      if (attrDef is UiAttributeDefinitionAtomic attrDefAtomic) {
        Write(writer, "Data Type", attrDefAtomic.DataType.Name);
        Write(writer, "Default Value", attrDefAtomic.DefaultValue?.ToString());
        Write(writer, "Attached Attribute", attrDefAtomic.IsAttached ? "Yes" : null);
      }
    }

    private void Write(TextWriter writer, string label, string value) {
      if (!string.IsNullOrWhiteSpace(value))
        writer.WriteLine("{0}: {1}", label, value);
    }
    #endregion

    #region Overrides
    public override string ToString() {
      return "UI Library: " + Name;
    }
    #endregion
  }
}

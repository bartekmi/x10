using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using x10.parsing;
using x10.model.metadata;
using x10.model.definition;
using System.IO;

namespace x10.ui.metadata {

  public enum UseMode {
    ReadOnly,
    ReadWrite,
  }

  public class UiLibrary {
    #region Properties
    public string Name { get; set; }
    public string Description { get; set; }

    private readonly Dictionary<string, ClassDef> _definitionsByName;
    private HashSet<ClassDef> _wrapperComponents;

    private readonly Dictionary<DataType, ClassDef> _dataTypesToComponentRW;
    private readonly Dictionary<DataType, ClassDef> _dataTypesToComponentRO;

    private ClassDef _defaultComponentForEnumsRW;
    private ClassDef _defaultComponentForEnumsRO;

    private ClassDef _defaultComponentForAssociationsRW;
    private ClassDef _defaultComponentForAssociationsRO;

    // Derived
    public IEnumerable<ClassDef> All => _definitionsByName.Values;
    public IEnumerable<string> AllNames => _definitionsByName.Keys;
    #endregion

    #region Creating the Library
    // Constructor
    public UiLibrary(IEnumerable<ClassDef> definitions) {
      _definitionsByName = definitions.ToDictionary(x => x.Name);
      _dataTypesToComponentRW = new Dictionary<DataType, ClassDef>();
      _dataTypesToComponentRO = new Dictionary<DataType, ClassDef>();

      // Add enums from libraries
      foreach (ClassDef classDef in definitions)
        foreach (UiAttributeDefinition attrDef in classDef.LocalAttributeDefinitions)
          if (attrDef is UiAttributeDefinitionAtomic atomic)
            if (atomic.DataType is DataTypeEnum)
              DataTypes.Singleton.AddDataType(atomic.DataType);
    }

    public void AddDataTypeToComponentAssociation(DataType dataType, string componentName, UseMode mode) {
      ClassDef uiComponent = FindComponentByName(componentName);
      if (uiComponent == null)
        throw new Exception(string.Format("Attempting to set default component for data type {0}. Component {1} does not exist",
          dataType.Name, componentName));

      switch (mode) {
        case UseMode.ReadOnly: _dataTypesToComponentRO[dataType] = uiComponent; break;
        case UseMode.ReadWrite: _dataTypesToComponentRW[dataType] = uiComponent; break;
        default:
          throw new NotImplementedException("Unexpected mode: " + mode);
      }
    }

    public void SetComponentForEnums(string componentName, UseMode mode) {
      ClassDef uiComponent = FindComponentByName(componentName);
      if (uiComponent == null)
        throw new Exception(string.Format("Attempting to set default component for enums. Component {0} does not exist. Mode: {1}",
          componentName, mode));

      switch (mode) {
        case UseMode.ReadOnly: _defaultComponentForEnumsRO = uiComponent; break;
        case UseMode.ReadWrite: _defaultComponentForEnumsRW = uiComponent; break;
        default:
          throw new NotImplementedException("Unexpected mode: " + mode);
      }
    }

    public void SetComponentForAssociations(string componentName, UseMode mode) {
      ClassDef uiComponent = FindComponentByName(componentName);
      if (uiComponent == null)
        throw new Exception(string.Format("Attempting to set default component for associations. Component {0} does not exist. Mode: {1}",
          componentName, mode));

      switch (mode) {
        case UseMode.ReadOnly: _defaultComponentForAssociationsRO = uiComponent; break;
        case UseMode.ReadWrite: _defaultComponentForAssociationsRW = uiComponent; break;
        default:
          throw new NotImplementedException("Unexpected mode: " + mode);
      }
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

    public ClassDef FindUiComponentForMember(Member member, UseMode mode) {
      if (member is X10Attribute attribute)
        return FindUiComponentForDataType(attribute, mode);
      else if (member is Association) {
        switch (mode) {
          case UseMode.ReadOnly: return _defaultComponentForAssociationsRO;
          case UseMode.ReadWrite: return _defaultComponentForAssociationsRW;
          default:
            throw new NotImplementedException("Unexpected mode: " + mode);
        }
      } else
        throw new Exception("Unexpected member type: " + member.GetType().Name);
    }

    private ClassDef FindUiComponentForDataType(X10Attribute attribute, UseMode mode) {
      if (attribute.DataType == null)
        return null;
        
      ClassDef uiComponent = null;

      switch (mode) {
        case UseMode.ReadOnly:
          _dataTypesToComponentRO.TryGetValue(attribute.DataType, out uiComponent);
          if (uiComponent == null && attribute.DataType is DataTypeEnum)
            uiComponent = _defaultComponentForEnumsRO;
          break;
        case UseMode.ReadWrite:
          _dataTypesToComponentRW.TryGetValue(attribute.DataType, out uiComponent);
          if (uiComponent == null && attribute.DataType is DataTypeEnum)
            uiComponent = _defaultComponentForEnumsRW;
          break;
        default:
          throw new NotImplementedException("Unexpected mode: " + mode);
      }

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

    private string ToMarkdown(string text) {
      if (text == null)
        return null;

      return text.Replace("<", "*").Replace(">", "*");
    }

    private void GenerateMarkdownForClassDef(TextWriter writer, ClassDef classDef) {
      writer.WriteLine("# Class Definition - '{0}'", classDef.Name);
      writer.WriteLine();
      writer.WriteLine(ToMarkdown(classDef.Description));
      writer.WriteLine();

      Write(writer, "Inherits From", classDef.InheritsFrom?.Name);
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
        attrDef.IsPrimary ? " (Primary)" : null);
      writer.WriteLine();
      writer.WriteLine(attrDef.Description);
      writer.WriteLine();

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
        writer.WriteLine("- {0}: {1}", label, value);
    }
    #endregion

    #region Overrides
    public override string ToString() {
      return "UI Library: " + Name;
    }
    #endregion
  }
}

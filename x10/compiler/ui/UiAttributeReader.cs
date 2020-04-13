using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using x10.parsing;
using x10.model.metadata;
using x10.ui.metadata;
using x10.ui.composition;
using x10.model;

namespace x10.compiler {

  internal enum UiAttributeReaderPass {
    Pass2_1,
    Pass2_2,
  }

  internal class UiAttributeReader {

    private readonly MessageBucket _messages;
    private readonly AllEntities _allEntities;
    private readonly AllEnums _allEnums;

    internal UiAttributeReader(MessageBucket messages, AllEntities allEntities, AllEnums allEnums) {
      _messages = messages;
      _allEntities = allEntities;
      _allEnums = allEnums;
    }

    internal void ReadAttributesForClassDef(ClassDefX10 classDef) {
      ReadAttributesPrivate(UiAppliesTo.UiDefinition, classDef, UiAttributeDefinitions.All);
    }

    internal void ReadAttributesForInstance(Instance instance, params string[] attributesToExclude) {
      IEnumerable<UiAttributeDefinition> classDefAttrs = instance.RenderAs?.AtomicAttributeDefinitions;
      IEnumerable<UiAttributeDefinition> allAttrs = classDefAttrs == null ?
        UiAttributeDefinitions.All :
        UiAttributeDefinitions.All.Concat(classDefAttrs);

      allAttrs = allAttrs.Where(x => !attributesToExclude.Contains(x.Name));

      UiAppliesTo appliesTo = instance is InstanceModelRef ? UiAppliesTo.UiModelReference : UiAppliesTo.UiComponentUse;

      ReadAttributesPrivate(appliesTo, instance, allAttrs);
    }

    internal void ReadSpecificAttributes(IAcceptsUiAttributeValues modelComponent,
      UiAppliesTo appliesTo,
      params string[] attributeNames
      ) {

      foreach (string attributeName in attributeNames) {
        UiAttributeDefinition attrDef = UiAttributeDefinitions.FindAttribute(appliesTo, attributeName);
        ReadAttribute(modelComponent, attrDef);
      }
    }

    private void ReadAttributesPrivate(UiAppliesTo appliesTo, IAcceptsUiAttributeValues modelComponent, IEnumerable<UiAttributeDefinition> attrDefs) {
      foreach (UiAttributeDefinition attrDef in attrDefs)
        if (attrDef.AppliesToType(appliesTo))
          ReadAttribute(modelComponent, attrDef);
    }

    private void ReadAttribute(
      IAcceptsUiAttributeValues modelComponent,
      UiAttributeDefinition attrDef) {

      // Error if mandatory attribute missing
      XmlElement xmlElement = modelComponent.XmlElement;
      XmlAttribute attrNode = xmlElement.FindAttribute(attrDef.Name);
      object typedValue;

      if (attrNode == null) {
        if (attrDef.DefaultValue == null) {
          if (attrDef.IsMandatory) {
            string classDefOwnership = attrDef.Owner == null ?
              "" :
              string.Format(" of Class Definition '{0}'", attrDef.Owner.Name);

            _messages.AddError(xmlElement,
              string.Format("Mandatory Atomic Attribute '{0}'{1} is missing",
              attrDef.Name, classDefOwnership));
          }
          return;
        } else
          typedValue = attrDef.DefaultValue;
      } else {
        if (attrDef is UiAttributeDefinitionAtomic attrPrimitive) {
          // Attempt to parse the string attribute value according to its data type
          DataType dataType = attrPrimitive.DataType;
          typedValue = dataType.Parse(attrNode.Value.ToString(), _messages, attrNode.Value, attrDef.Name);
          if (typedValue == null)
            return;
        } else if (attrDef is UiAttributeDefinitionComplex attrComplex) {
          // TODO... Not yet sure if/how we'll handle complex attributes at this level
          throw new NotImplementedException();
        } else
          throw new Exception("Wrong attribute type: " + attrDef.GetType().Name);
      }

      // If a setter has been provided, use it; 
      if (attrDef.Setter != null) {
        Type modelComponentType = modelComponent.GetType();
        PropertyInfo info = attrDef.GetPropertyInfo(modelComponentType);
        if (info == null) {
          throw new Exception(string.Format("Setter property '{0}' on Model Attribute Definition '{1}' does not exist on type {2}",
            attrDef.Setter, attrDef.Name, modelComponentType.Name));
        }
        info.SetValue(modelComponent, typedValue);
      }

      if (attrNode != null) {   // It is null if default was used
        // A ModelAttributeValue is always stored, even if a setter exists. For one thing,
        // this is the only way we can track where the attribute came from in the code.
        UiAttributeValueAtomic attrValue = (UiAttributeValueAtomic)attrDef.CreateValueAndAddToOwner(modelComponent, attrNode.Value);
        attrValue.Value = typedValue;

        // Do Pass-1 action, if one exists
        attrDef.Pass1Action?.Invoke(_messages, _allEntities, _allEnums, attrNode.Value, modelComponent);
      }
    }
  }
}

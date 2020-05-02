using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using x10.parsing;
using x10.model.metadata;
using x10.ui.metadata;
using x10.ui.composition;
using x10.model;
using System.Text.RegularExpressions;

namespace x10.compiler {

  internal enum UiAttributeReaderPass {
    Pass2_1,
    Pass2_2,
  }

  internal class UiAttributeReader {

    private readonly MessageBucket _messages;
    private readonly AllEntities _allEntities;
    private readonly AllEnums _allEnums;
    private readonly AllUiDefinitions _allClassDefs;

    internal UiAttributeReader(MessageBucket messages, AllEntities allEntities, AllEnums allEnums, AllUiDefinitions allClassDefs) {
      _messages = messages;
      _allEntities = allEntities;
      _allEnums = allEnums;
      _allClassDefs = allClassDefs;
    }

    internal void ReadAttributesForClassDef(ClassDefX10 classDef) {
      ReadAttributesPrivate(UiAppliesTo.ClassDef, classDef, UiAttributeDefinitions.All, new string[0], true);
    }

    internal void ReadAttributesForInstance(Instance instance, params string[] attributesToExclude) {
      IEnumerable<UiAttributeDefinitionAtomic> classDefAttrs = instance.RenderAs?.AtomicAttributeDefinitions;
      bool classDefKnown = classDefAttrs != null;
      IEnumerable<UiAttributeDefinitionAtomic> allAttrs = classDefKnown ?
        UiAttributeDefinitions.All.Concat(classDefAttrs) :
        UiAttributeDefinitions.All;

      UiAppliesTo appliesTo = instance is InstanceModelRef ? UiAppliesTo.UiModelReference : UiAppliesTo.UiComponentUse;

      ReadAttributesPrivate(appliesTo, instance, allAttrs, attributesToExclude, classDefKnown);
      ReadAttachedAttributes(instance);
    }

    internal void ReadSpecificAttributes(IAcceptsUiAttributeValues modelComponent,
      UiAppliesTo appliesTo,
      params string[] attributeNames
      ) {

      foreach (string attributeName in attributeNames) {
        UiAttributeDefinitionAtomic attrDef = UiAttributeDefinitions.FindAttribute(appliesTo, attributeName);
        ReadAttribute(modelComponent, attrDef);
      }
    }

    private void ReadAttributesPrivate(UiAppliesTo appliesTo,
      IAcceptsUiAttributeValues modelComponent,
      IEnumerable<UiAttributeDefinitionAtomic> attrDefs,
      string[] attributesToExclude,
      bool errorOnUnknownAttributes) {

      IEnumerable<UiAttributeDefinitionAtomic> applicableAttrDefs = attrDefs.Where(x => x.AppliesToType(appliesTo));
      IEnumerable<UiAttributeDefinitionAtomic> applicableMinusExcluded = applicableAttrDefs.Where(x => !attributesToExclude.Contains(x.Name));

      foreach (UiAttributeDefinitionAtomic attrDef in applicableMinusExcluded)
        ReadAttribute(modelComponent, attrDef);

      if (errorOnUnknownAttributes)
        ErrorOnUnknownAttributes(modelComponent, applicableAttrDefs);
    }

    private void ReadAttribute(
      IAcceptsUiAttributeValues modelComponent,
      UiAttributeDefinitionAtomic attrDef) {

      // Error if mandatory attribute missing
      XmlElement xmlElement = modelComponent.XmlElement;
      XmlAttribute xmlAttribute = xmlElement.FindAttribute(attrDef.Name);

      // Mising mandatory attribute
      if (xmlAttribute == null && attrDef.DefaultValue == null && attrDef.IsMandatory) {
        string classDefOwnership = attrDef.Owner == null ?
          "" :
          string.Format(" of Class Definition '{0}'", attrDef.Owner.Name);

        _messages.AddError(xmlElement,
          string.Format("Mandatory Atomic Attribute '{0}'{1} is missing",
          attrDef.Name, classDefOwnership));
        return;
      }

      object typedValue = attrDef.DefaultValue;
      if (xmlAttribute != null)
        typedValue = ParseAttributeAndAddToInstance(attrDef, xmlAttribute, modelComponent);

      // If a setter has been provided, use it; 
      if (attrDef.Setter != null) {
        // TODO: Error if user attempted to provide a formula

        Type modelComponentType = modelComponent.GetType();
        PropertyInfo info = attrDef.GetPropertyInfo(modelComponentType);
        if (info == null) {
          throw new Exception(string.Format("Setter property '{0}' on Model Attribute Definition '{1}' does not exist on type {2}",
            attrDef.Setter, attrDef.Name, modelComponentType.Name));
        }
        info.SetValue(modelComponent, typedValue);
      }
    }

    private void ErrorOnUnknownAttributes(IAcceptsUiAttributeValues modelComponent, IEnumerable<UiAttributeDefinitionAtomic> applicableAttrDefs) {
      HashSet<string> validAttributeNames = new HashSet<string>(applicableAttrDefs.Select(x => x.Name));

      foreach (XmlAttribute xmlAttribute in modelComponent.XmlElement.Attributes) {
        if (IsAttachedAttribute(xmlAttribute.Key, out _, out _))
          continue;

        if (!validAttributeNames.Contains(xmlAttribute.Key))
          _messages.AddError(xmlAttribute,
            string.Format("Unknown attribute '{0}' on Class Definition '{1}'", xmlAttribute.Key, modelComponent.ClassDef?.Name));
      }
    }

    private void ReadAttachedAttributes(Instance instance) {
      foreach (XmlAttribute xmlAttribute in instance.XmlElement.Attributes) {
        if (IsAttachedAttribute(xmlAttribute.Key, out string ownerClassDefName, out string attrName)) {
          ClassDef classDef = _allClassDefs.FindDefinitionByNameWithError(ownerClassDefName, xmlAttribute);
          if (classDef == null)
            continue;

          UiAttributeDefinitionAtomic attrDef = classDef.FindAtomicAttribute(attrName);
          if (attrDef == null) {
            _messages.AddError(xmlAttribute, "Atomic attribute '{0}' not found on Class Definition '{1}'",
              attrName, classDef.Name);
            continue;
          }

          ParseAttributeAndAddToInstance(attrDef, xmlAttribute, instance);
        }
      }
    }

    private object ParseAttributeAndAddToInstance(UiAttributeDefinitionAtomic attrDef, XmlAttribute xmlAttribute, IAcceptsUiAttributeValues modelComponent) {
      object typedValue = attrDef.DefaultValue;
      string formula = null;

      // Parse value (or don't if it's a formula)
      if (attrDef is UiAttributeDefinitionAtomic attrPrimitive) {
        DataType dataType = attrPrimitive.DataType;
        string attrValue = xmlAttribute.Value.ToString();
        if (IsFormula(attrValue))
          formula = attrValue;
        else {
          typedValue = dataType.Parse(attrValue, _messages, xmlAttribute.Value, attrDef.Name);
          if (typedValue == null)
            return null;
        }
      } else
        throw new Exception("Wrong attribute type: " + attrDef.GetType().Name);

      if (xmlAttribute != null) {   // It is null if default was used
        UiAttributeValueAtomic attrValue = attrDef.CreateValueAndAddToOwnerAtomic(modelComponent, xmlAttribute.Value);
        attrValue.Value = typedValue;
        attrValue.Formula = formula;

        // Do Pass-1 action, if one exists
        attrDef.Pass1Action?.Invoke(_messages, _allEntities, _allEnums, xmlAttribute.Value, modelComponent);
      }

      return typedValue;
    }


    private static bool IsFormula(string valueOrFormula) {
      return valueOrFormula.Trim().StartsWith("=");
    }

    public static bool IsAttachedAttribute(string attributeName, out string ownerClassDefName, out string attrName) {
      ownerClassDefName = null;
      attrName = null;

      string[] pieces = attributeName.Split('.');
      if (pieces.Length != 2)
        return false;

      if (ModelValidationUtils.IsUiElementName(pieces[0]) && ModelValidationUtils.IsUiAtomicAttributeName(pieces[1])) {
        ownerClassDefName = pieces[0];
        attrName = pieces[1];
        return true;
      }

      return false;
    }
  }
}

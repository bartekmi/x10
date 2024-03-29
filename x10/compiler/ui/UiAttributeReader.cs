﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using x10.formula;
using x10.model;
using x10.model.definition;
using x10.model.metadata;
using x10.parsing;
using x10.ui.composition;
using x10.ui.metadata;

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
      IEnumerable<UiAttributeDefinitionAtomic> attrDefs = UiAttributeDefinitions.AllApplicable(UiAppliesTo.ClassDef);
      ReadAttributesPrivate(classDef.XmlElement, classDef, attrDefs, null, new string[0]);
      ErrorOnUnknownAttributes(classDef, attrDefs);
    }

    internal void ReadAttributesForInstance(Instance instance, Instance wrapper, params string[] attributesToExclude) {
      XmlElement source = instance.XmlElement;

      // If wrapper was given, extract the wrapper attributes, first
      IEnumerable<UiAttributeDefinitionAtomic> attributesHoistedToWrapper = new List<UiAttributeDefinitionAtomic>();
      if (wrapper != null) {
        IEnumerable<UiAttributeDefinitionAtomic> wrapperAttrDefs = wrapper.RenderAs.AtomicAttributeDefinitions;
        attributesHoistedToWrapper = ReadAttributesPrivate(source, wrapper, wrapperAttrDefs, instance.ModelMember, attributesToExclude);
      }

      // Now, extract the actual component attributes
      IEnumerable<UiAttributeDefinitionAtomic> classDefAttrs = instance.RenderAs?.AtomicAttributeDefinitions;
      UiAppliesTo appliesTo = instance is InstanceModelRef ? UiAppliesTo.UiModelReference : UiAppliesTo.UiComponentUse;
      IEnumerable<UiAttributeDefinitionAtomic> attrDefs = UiAttributeDefinitions.AllApplicable(appliesTo);
      if (classDefAttrs != null)
        attrDefs = attrDefs.Concat(classDefAttrs);

      IEnumerable<string> exclude = attributesToExclude.Concat(attributesHoistedToWrapper.Select(x => x.Name));

      ReadAttributesPrivate(source, instance, attrDefs, instance.ModelMember, exclude);
      if (classDefAttrs != null)
        ErrorOnUnknownAttributes(instance, attrDefs.Concat(attributesHoistedToWrapper));

      ReadAttachedAttributes(instance);
    }

    internal void ReadSpecificAttributes(IAcceptsUiAttributeValues modelComponent,
      UiAppliesTo appliesTo,
      params string[] attributeNames
      ) {

      foreach (string attributeName in attributeNames) {
        UiAttributeDefinitionAtomic attrDef = UiAttributeDefinitions.FindAttribute(appliesTo, attributeName);
        ReadAttribute(modelComponent.XmlElement, modelComponent, attrDef, null);
      }
    }

    private IEnumerable<UiAttributeDefinitionAtomic> ReadAttributesPrivate(
      XmlElement source,
      IAcceptsUiAttributeValues recipient,
      IEnumerable<UiAttributeDefinitionAtomic> attrDefs,
      Member takeAttributesFromMember,            // Some attributes can be taken from Member, if missing
      IEnumerable<string> attributesToExclude) {

      IEnumerable<UiAttributeDefinitionAtomic> applicableMinusExcluded = attrDefs.Where(x => !attributesToExclude.Contains(x.Name));

      List<UiAttributeDefinitionAtomic> attributesRead = new List<UiAttributeDefinitionAtomic>();
      foreach (UiAttributeDefinitionAtomic attrDef in applicableMinusExcluded)
        if (ReadAttribute(source, recipient, attrDef, takeAttributesFromMember))
          attributesRead.Add(attrDef);

      return attributesRead;
    }

    private bool ReadAttribute(
      XmlElement source,
      IAcceptsUiAttributeValues recipient,
      UiAttributeDefinitionAtomic attrDef,
      Member takeAttributeFromMember) {

      // If Primary Attribute, could be specified as content
      XmlScalar xmlScalar = source.FindAttribute(attrDef.Name)?.Value;
      if (attrDef.IsPrimary) {
        XmlScalar xmlScalarFromText = source.TextContent;
        if (xmlScalarFromText != null) {
          if (xmlScalar != null) {
            _messages.AddError(xmlScalar, "Attribute '{0}' specified as both Attribute and content", attrDef.Name);
            return false;
          }
          xmlScalar = xmlScalarFromText;
        }
      }

      // Some UI Attributes can be derived from Model Attributes (e.g. label, etc).
      // Attempt to do so now.
      if (xmlScalar == null && attrDef.TakeValueFromModelAttr != null)
        xmlScalar = DeriveAttributeValueFromModel(takeAttributeFromMember, attrDef.TakeValueFromModelAttr, source);

      // Mising mandatory attribute
      if (xmlScalar == null && attrDef.DefaultValue == null && attrDef.IsMandatory) {
        string classDefOwnership = attrDef.Owner == null ?
          "" :
          string.Format(" of Class Definition '{0}'", attrDef.Owner.Name);

        _messages.AddError(source, "Mandatory Atomic Attribute '{0}'{1} is missing",
          attrDef.Name, classDefOwnership);
        return false;
      }

      object typedValue = attrDef.DefaultValue;
      if (xmlScalar != null)
        typedValue = ParseAttributeAndAddToInstance(attrDef, xmlScalar, recipient);

      // If a setter has been provided, use it; 
      if (attrDef.Setter != null) {
        // TODO: Error if user attempted to provide a formula

        Type modelComponentType = recipient.GetType();
        PropertyInfo info = attrDef.GetPropertyInfo(modelComponentType);
        if (info == null) {
          throw new Exception(string.Format("Setter property '{0}' on Model Attribute Definition '{1}' does not exist on type {2}",
            attrDef.Setter, attrDef.Name, modelComponentType.Name));
        }
        info.SetValue(recipient, typedValue);
      }

      return xmlScalar != null;
    }

    private XmlScalar DeriveAttributeValueFromModel(Member takeAttributeFromMember, ModelAttributeDefinition attrDef, XmlElement source) {
      if (takeAttributeFromMember == null)
        return null;
      object value = takeAttributeFromMember.FindValue(attrDef.Name);
      if (value == null)
        return null;

      return new XmlScalar(source, value.ToString()); // TODO: This is slightly misleading, as any errors reported will not point to Entity YAML file
    }

    private void ErrorOnUnknownAttributes(IAcceptsUiAttributeValues recipient, IEnumerable<UiAttributeDefinitionAtomic> validAttributes) {
      HashSet<string> validAttributeNames = new HashSet<string>(validAttributes.Select(x => x.Name));

      foreach (XmlAttribute xmlAttribute in recipient.XmlElement.Attributes) {
        if (IsAttachedAttribute(xmlAttribute.Key, out _, out _))
          continue;

        if (!validAttributeNames.Contains(xmlAttribute.Key))
          _messages.AddErrorDidYouMean(xmlAttribute, xmlAttribute.Key, validAttributeNames,
            "Unknown attribute '{0}' on Class Definition '{1}'", xmlAttribute.Key, recipient.ClassDef?.Name);
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

          ParseAttributeAndAddToInstance(attrDef, xmlAttribute.Value, instance);
        }
      }
    }

    private object ParseAttributeAndAddToInstance(
      UiAttributeDefinitionAtomic attrDef,
      XmlScalar xmlScalar,
      IAcceptsUiAttributeValues modelComponent) {

      object typedValue = attrDef.DefaultValue;
      string formula = null;

      // Parse value (or don't if it's a formula)
      if (attrDef is UiAttributeDefinitionAtomic attrPrimitive) {
        DataType dataType = attrPrimitive.DataType;
        string attrValue = xmlScalar.ToString();

        if (FormulaUtils.IsFormula(attrValue, out string strippedFormula))
          formula = strippedFormula;
        else {
          typedValue = dataType.Parse(attrValue, _messages, xmlScalar, attrDef.Name);
          if (typedValue == null)
            return null;
        }
      } else
        throw new Exception("Wrong attribute type: " + attrDef.GetType().Name);

      if (xmlScalar != null) {   // It is null if default was used
        UiAttributeValueAtomic attrValue = attrDef.CreateValueAndAddToOwnerAtomic(modelComponent, xmlScalar);
        attrValue.Value = typedValue;
        attrValue.Formula = formula;

        // Do Pass-1 action, if one exists
        attrDef.Pass1Action?.Invoke(_messages, _allEntities, _allEnums, xmlScalar, modelComponent);
      }

      return typedValue;
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

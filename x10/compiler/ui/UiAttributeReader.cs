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

    internal UiAttributeReader(MessageBucket messages, AllEntities allEntities, AllEnums allEnums) {
      _messages = messages;
      _allEntities = allEntities;
      _allEnums = allEnums;
    }

    internal void ReadAttributesForClassDef(ClassDefX10 classDef) {
      ReadAttributesPrivate(UiAppliesTo.ClassDef, classDef, UiAttributeDefinitions.All, new string[0], true);
    }

    internal void ReadAttributesForInstance(Instance instance, params string[] attributesToExclude) {
      IEnumerable<UiAttributeDefinition> classDefAttrs = instance.RenderAs?.AtomicAttributeDefinitions;
      bool classDefKnown = classDefAttrs != null;
      IEnumerable<UiAttributeDefinition> allAttrs = classDefKnown ?
        UiAttributeDefinitions.All.Concat(classDefAttrs) :
        UiAttributeDefinitions.All;

      UiAppliesTo appliesTo = instance is InstanceModelRef ? UiAppliesTo.UiModelReference : UiAppliesTo.UiComponentUse;

      ReadAttributesPrivate(appliesTo, instance, allAttrs, attributesToExclude, classDefKnown);
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

    private void ReadAttributesPrivate(UiAppliesTo appliesTo,
      IAcceptsUiAttributeValues modelComponent,
      IEnumerable<UiAttributeDefinition> attrDefs,
      string[] attributesToExclude,
      bool errorOnUnknownAttributes) {

      IEnumerable<UiAttributeDefinition> applicableAttrDefs = attrDefs.Where(x => x.AppliesToType(appliesTo));
      IEnumerable<UiAttributeDefinition> applicableMinusExcluded = applicableAttrDefs.Where(x => !attributesToExclude.Contains(x.Name));

      foreach (UiAttributeDefinition attrDef in applicableMinusExcluded)
        ReadAttribute(modelComponent, attrDef);

      if (errorOnUnknownAttributes)
        ErrorOnUnknownAttributes(modelComponent, applicableAttrDefs);
    }

    private void ReadAttribute(
      IAcceptsUiAttributeValues modelComponent,
      UiAttributeDefinition attrDef) {

      // Error if mandatory attribute missing
      XmlElement xmlElement = modelComponent.XmlElement;
      XmlAttribute attrNode = xmlElement.FindAttribute(attrDef.Name);
      object typedValue = null;
      string formula = null;

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
          DataType dataType = attrPrimitive.DataType;
          string attrValue = attrNode.Value.ToString();
          if (IsFormula(attrValue))
            formula = attrValue;
          else {
            typedValue = dataType.Parse(attrValue, _messages, attrNode.Value, attrDef.Name);
            if (typedValue == null)
              return;
          }
        } else
          throw new Exception("Wrong attribute type: " + attrDef.GetType().Name);
      }

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

      if (attrNode != null) {   // It is null if default was used
        // A ModelAttributeValue is always stored, even if a setter exists. For one thing,
        // this is the only way we can track where the attribute came from in the code.
        UiAttributeValueAtomic attrValue = (UiAttributeValueAtomic)attrDef.CreateValueAndAddToOwner(modelComponent, attrNode.Value);
        attrValue.Value = typedValue;
        attrValue.Formula = formula;

        // Do Pass-1 action, if one exists
        attrDef.Pass1Action?.Invoke(_messages, _allEntities, _allEnums, attrNode.Value, modelComponent);
      }
    }

    private void ErrorOnUnknownAttributes(IAcceptsUiAttributeValues modelComponent, IEnumerable<UiAttributeDefinition> applicableAttrDefs) {
      HashSet<string> validAttributeNames = new HashSet<string>(applicableAttrDefs.Select(x => x.Name));

      foreach (XmlAttribute attribute in modelComponent.XmlElement.Attributes) {
        if (!validAttributeNames.Contains(attribute.Key))
          _messages.AddError(attribute,
            string.Format("Unknown attribute '{0}' on Class Definition '{1}'", attribute.Key, modelComponent.ClassDef?.Name));
      }
    }

    public static bool IsFormula(string valueOrFormula) {
      return valueOrFormula.Trim().StartsWith("=");
    }

    public static bool IsAttachedAttribute(string attributeName) {
      string[] pieces = attributeName.Split('.');
      if (pieces.Length != 2)
        return false;

      return ModelValidationUtils.IsUiElementName(pieces[0]) &&
        ModelValidationUtils.IsUiAtomicAttributeName(pieces[1]);
    }
  }
}

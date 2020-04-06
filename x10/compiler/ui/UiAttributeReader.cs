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
  internal class UiAttributeReader {

    private readonly MessageBucket _messages;
    private readonly AllEntities _allEntities;
    private readonly AllEnums _allEnums;

    internal UiAttributeReader(MessageBucket messages, AllEntities allEntities, AllEnums allEnums) {
      _messages = messages;
      _allEntities = allEntities;
      _allEnums = allEnums;
    }

    // Read the attributes of this element
    internal void ReadAttributes(XmlElement element, UiAppliesTo type, IAcceptsUiAttributeValues modelComponent) {
      // Iterate known attributes and extract
      // NOTE: Attributes must be checked in order. Don't convert this to a hash.
      // Reason: 'dataType' must be parsed before the 'default' attribute
      foreach (UiAttributeDefinition attrDef in UiAttributeDefinitions.All)
        if (attrDef.AppliesToType(type))
          ReadAttribute(element, type, modelComponent, attrDef);

      ErrorOnUnknownAttributes(element, type);
    }

    private void ReadAttribute(XmlElement element, UiAppliesTo type, IAcceptsUiAttributeValues modelComponent, UiAttributeDefinition attrDef) {
      // Error if mandatory attribute missing
      XmlAttribute attrNode = element.FindAttribute(attrDef.Name);
      if (attrNode == null) {
        if (attrDef.IsMandatory)
          _messages.AddError(element,
            string.Format("The attribute '{0}' is missing from {1}", attrDef.Name, type));
        return;
      }

      object typedValue;
      if (attrDef is UiAttributeDefinitionPrimitive attrPrimitive) {
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

      // A ModelAttributeValue is always stored, even if a setter exists. For one thing,
      // this is the only way we can track where the attribute came from in the code.
      UiAttributeValue attrValue = new UiAttributeValue(attrNode.Value) {
        Value = typedValue,
        Definition = attrDef,
      };
      modelComponent.AttributeValues.Add(attrValue);

      // Do validation, if requried
      attrDef.Pass1Action?.Invoke(_messages, _allEntities, _allEnums, attrNode.Value, modelComponent);
    }

    private void ErrorOnUnknownAttributes(XmlElement element, UiAppliesTo type) {
      HashSet<string> validAttributeNames =
        new HashSet<string>(UiAttributeDefinitions.All.Where(x => x.AppliesToType(type))
          .Select(x => x.Name));

      foreach (XmlAttribute attribute in element.Attributes)
        if (!validAttributeNames.Contains(attribute.Key))
          _messages.AddError(attribute,
            string.Format("Unknown attribute '{0}' on {1}", attribute.Key, type));
    }
  }
}

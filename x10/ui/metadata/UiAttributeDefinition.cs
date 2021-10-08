using System;
using System.Reflection;

using x10.parsing;
using x10.model;
using x10.model.metadata;

using x10.ui.composition;
using x10.compiler;

namespace x10.ui.metadata {

  public enum UiAppliesTo {
    None = 0,
    ClassDef = 1,
    UiModelReference = 2,
    UiComponentUse = 4,
  }

  public abstract class UiAttributeDefinition {
    public string Name { get; set; }
    public string Description { get; set; }

    // If specified, this determines what type of entity this attribute applies to
    // Normally, this is only used for attributes defined in 'UiAttributeDefinitions"
    public UiAppliesTo? AppliesTo { get; set; }

    // Optional default value for the attribute. Must match Data Type.
    public object DefaultValue { get; set; }

    // A UI Definition can have (at most) one "Primary" attribute - this is the attribute
    // that is used for the children in the XML tree
    public bool IsPrimary { get; set; }

    // If true, the property must be provided. An error is generated if a mandatory property is omitted 
    // and it has no default value.
    public bool IsMandatory { get; set; }

    // If true, multiple instances of values are allowed.
    // For complex attribute values, this means multiple Instances.
    // For atomic attribute values, the Value property will store an array
    public bool IsMany { get; set; }

    // Optional name of setter method if the attribute can be stored in code
    public string Setter { get; set; }

    // Some Ui Attribute Definitions, when used in the context of <InstanceModelRef>,
    // can be auto-populated from Entity Member Attributes. Classic example is that
    // <Label.label> gets populated from label property of Member
    public string TakeValueFromModelAttrName { get; set; }

    // Hyrdrated
    public ClassDef Owner { get; internal set; }
    public ModelAttributeDefinition TakeValueFromModelAttr { get; set; }

    public Action<MessageBucket, AllEntities, AllEnums, XmlScalar, IAcceptsUiAttributeValues> Pass1Action { get; set; }

    // runs During Pass 2 of UI Complation - BEFORE all other attributes from the XML file are read
    // Do not use this on attributes defined in libraries as it will not work.
    // Reserved for internal use only.
    public Action<MessageBucket, AllEntities, AllEnums, AllUiDefinitions, IAcceptsUiAttributeValues, UiAttributeValueAtomic> Pass2ActionPre { get; set; }

    // runs During Pass 2 of UI Complation - AFTER all attributes from the XML file are read
    public Action<MessageBucket, AllEntities, AllEnums, AllUiDefinitions, IAcceptsUiAttributeValues, UiAttributeValueAtomic> Pass2ActionPost { get; set; }

    protected UiAttributeDefinition() {
      // Some sensible defaults...
      IsPrimary = false;
      IsMandatory = false;
      IsMany = false;
    }

    public UiAttributeValue CreateValueAndAddToOwner(IAcceptsUiAttributeValues owner, XmlBase xmlBase) {
      UiAttributeValue value;
      if (this is UiAttributeDefinitionAtomic defAtomic)
        value = new UiAttributeValueAtomic(defAtomic, owner, xmlBase);
      else if (this is UiAttributeDefinitionComplex defComplex)
        value = new UiAttributeValueComplex(defComplex, owner, xmlBase);
      else
        throw new Exception("Unexpected type of attribute: " + GetType().Name);

      owner.AttributeValues.Add(value);

      return value;
    }

    public bool AppliesToType(UiAppliesTo type) {
      return AppliesTo == null || (AppliesTo.Value & type) > 0;
    }

    public PropertyInfo GetPropertyInfo(Type type) {
      if (Setter == null) return null;
      return type.GetProperty(Setter, BindingFlags.Public | BindingFlags.Instance);
    }

    public override string ToString() {
      return "UiAttributeDefinition: " + Name;
    }
  }
}

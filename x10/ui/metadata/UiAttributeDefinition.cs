using System;
using System.Reflection;

using x10.parsing;
using x10.model;
using x10.model.metadata;

using x10.ui.composition;
using x10.compiler;

namespace x10.ui.metadata {

  public enum UiAppliesTo {
    ClassDef = 1,
    UiModelReference = 2,
    UiComponentUse = 4,
  }

  public abstract class UiAttributeDefinition {
    public string Name { get; set; }
    public string Description { get; set; }
    public ClassDef Owner { get; internal set; }

    // If specified, this determines what type of entity this attribute applies to
    // Normally, this is only used for attributes defined in 'UiAttributeDefinitions"
    public UiAppliesTo? AppliesTo { get; set; }

    // Optional default value for the attribute.Must match Data Type
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

    // if true, this property should be treated as part of the state 
    // of the component for the purpose of code-generation.
    public bool IsState { get; set; }

    // Optional name of setter method if the attribute can be stored in code
    public string Setter { get; set; }

    public Action<MessageBucket, AllEntities, AllEnums, XmlScalar, IAcceptsUiAttributeValues> Pass1Action { get; set; }
    public Action<MessageBucket, AllEntities, AllEnums, AllUiDefinitions, IAcceptsUiAttributeValues, UiAttributeValueAtomic> Pass2Action { get; set; }

    protected UiAttributeDefinition() {
      // Some sensible defaults...
      AppliesTo = UiAppliesTo.UiComponentUse;
      IsPrimary = false;
      IsMandatory = false;
      IsMany = false;
      IsState = false;
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

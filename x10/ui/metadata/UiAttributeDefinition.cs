using System;
using System.Reflection;

using x10.parsing;
using x10.model;
using x10.model.metadata;

using x10.ui.composition;
using x10.compiler;

namespace x10.ui.metadata {

  public enum UiAppliesTo {
    UiDefinition = 1,
    UiModelReference = 2,
    UiComponentUse = 4,
  }

  public abstract class UiAttributeDefinition {
    public string Name { get; set; }
    public string Description { get; set; }

    public UiAppliesTo AppliesTo { get; set; }

    // Optional default value for the attribute.Must match Data Type
    public object DefaultValue { get; set; }

    // A UI Component may have a maximum of one “writeable” attribute - it is the attribute 
    // which actually changes a property of the attached data Entity instance. 
    // An example would be the Text property for a TextInput control.
    public bool IsPrimary { get; set; }

    // If true, the property must be provided. An error is generated if a mandatory property is omitted 
    // and it has no default value.
    public bool IsMandatory { get; set; }

    // if true, this property should be treated as part of the state 
    // of the component for the purpose of code-generation.
    public bool IsState { get; set; }

    // Optional name of setter method if the attribute can be stored in code
    public string Setter { get; set; }

    public Action<MessageBucket, AllEntities, AllEnums, XmlScalar, IAcceptsUiAttributeValues> Pass1Action { get; set; }
    public Action<MessageBucket, AllEntities, AllEnums, AllUiDefinitions, IAcceptsUiAttributeValues, UiAttributeValue> Pass2Action { get; set; }

    public bool AppliesToType(UiAppliesTo type) {
      return (AppliesTo & type) > 0;
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

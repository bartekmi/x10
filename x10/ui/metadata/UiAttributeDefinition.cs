using System;
using System.Collections.Generic;
using System.Text;

namespace x10.ui.metadata {
  public abstract class UiAttributeDefinition {
    public string Name { get; set; }
    public string Description { get; set; }

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
  }
}

using System;
using System.Collections.Generic;
using System.Text;
using x10.model.definition;
using x10.ui.composition;
using x10.utils;

namespace x10.ui.platform {
  // A static attribute always added to an element when code-generating
  public class PlatformAttributeStatic : PlatformAttribute {

    // More values to be added in the future
    public const string PLURALIZED_DATA_TYPE = "$PLURALIZED_DATA_TYPE$";

    // Value of the attribute
    public string Value { get; set; }

    // If true, run a standard set of substitutions on the Value string
    public bool DoSubstitutions { get; set; }

    public string ValueWithSubstitutions(Instance instance) {
      string value = Value;
      string dataType = (instance.ModelMember as X10Attribute)?.DataType?.Name;
      
      if (DoSubstitutions) {
        if (dataType != null)
          value = value.Replace(PLURALIZED_DATA_TYPE, NameUtils.Pluralize(dataType));
      }

      return value;
    }
  }
}

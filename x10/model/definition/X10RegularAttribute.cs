using System;
using System.Collections.Generic;
using System.Text;
using x10.model.libraries;
using x10.model.definition;

namespace x10.model.definition {
  public class X10RegularAttribute : X10Attribute {
    // Derived
    public object DefaultValue {
      get {
        ModelAttributeValue defaultValue = this.FindAttribute(BaseLibrary.DEFAULT);
        return defaultValue == null ? null : defaultValue.Value;
      }
    }
  }
}

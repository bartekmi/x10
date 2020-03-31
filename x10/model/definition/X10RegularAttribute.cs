using System;
using System.Collections.Generic;
using System.Text;

namespace x10.model.definition {
  public class X10RegularAttribute : X10Attribute {
    public String DefaultValueAsString { get; set; }
    public object DefaultValue { get; set; }
  }
}

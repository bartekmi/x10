using System;
using System.Collections.Generic;
using System.Text;

namespace x10_csharp {
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field)]
  public class DefaultValue : Attribute {
    public object Value;

    public DefaultValue(object value) {
      Value = value;
    }
  }
}

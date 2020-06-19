using System;
using System.Collections.Generic;
using System.Text;
using x10.model.libraries;

namespace x10.model.definition {
  public class X10RegularAttribute : X10Attribute {
    public object DefaultValue { 
      get { return this.FindValue(BaseLibrary.DEFAULT); }
    }
  }
}

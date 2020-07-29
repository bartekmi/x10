using System;
using System.Collections.Generic;
using System.Text;
using x10.ui.composition;

namespace x10.ui.platform {
  public class PlatformAttributeByFunc : PlatformAttribute {
    // The value of the attribute is directly given by this function
    public Func<Instance, string> Function { get; set; }
  }
}

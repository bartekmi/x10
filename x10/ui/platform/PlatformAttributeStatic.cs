using System;
using System.Collections.Generic;
using System.Text;
using x10.model.definition;
using x10.ui.composition;
using x10.utils;

namespace x10.ui.platform {
  // A static attribute always added to an element when code-generating
  public class PlatformAttributeStatic : PlatformAttribute {

    // Value of the attribute
    public string Value { get; set; }
  }
}

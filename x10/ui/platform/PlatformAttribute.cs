using System;
using System.Collections.Generic;
using System.Text;

namespace x10.ui.platform {
  // Base class for static and dynamic platform-specific attributes
  public abstract class PlatformAttribute {
    // Name of this attribute in the platform-specific world.
    // Used for actual code-generation
    public string PlatformName { get; set; }
  }
}

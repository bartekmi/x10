using System;
using System.Collections.Generic;
using System.Text;

namespace x10.ui.platform {
  // Used when enum conversion is ncessary from the logical to the platform-specific
  // model.
  public class EnumConversion {
    public string From { get; private set; }

    // If this is null - do not generate the attribute - it is the default
    public object To { get; private set; }

    public EnumConversion(string from, object to) {
      From = from;
      To = to;
    }
  }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace x10_csharp {
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field)]
  public class ToolTip : Attribute {
    public string Text;

    public ToolTip(string label) {
      Text = label;
    }
  }
}

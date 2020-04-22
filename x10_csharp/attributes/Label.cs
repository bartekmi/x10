using System;
using System.Collections.Generic;
using System.Text;

namespace x10_csharp {
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Enum)]
  public class Label : Attribute {
    public string Text;

    public Label(string label) {
      Text = label;
    }
  }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace x10_csharp {
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field)]
  public class PlaceholderText : Attribute {
    public string Text;

    public PlaceholderText(string placeholderText) {
      Text = placeholderText;
    }
  }
}

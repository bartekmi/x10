using System;
using System.Collections.Generic;
using System.Text;

namespace x10_csharp {
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Enum)]
  public class Description : Attribute {
    public string Content;

    public Description(string description) {
      Content = description;
    }
  }
}

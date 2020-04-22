using System;
using System.Collections.Generic;
using System.Text;

namespace x10_csharp {
  [AttributeUsage(AttributeTargets.Field)]
  public class ReadOnly : Attribute {
  }
}

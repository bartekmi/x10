using System;
using System.Collections.Generic;
using System.Text;
using x10_csharp.ui;

namespace x10_csharp {
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field)]
  public class Ui : Attribute {
    public Type UiType { get; private set; }

    public Ui(Type type) {
      UiType = type;
    }
  }
}

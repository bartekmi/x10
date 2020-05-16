using System;
using System.Collections.Generic;
using System.Text;

using x10.ui.metadata;

namespace x10.model.definition {
  public abstract class Member : ModelComponent {
    public bool IsMandatory { get; set; }
    public bool IsReadOnly { get; set; }
    public Entity Owner { get; internal set; }
    public string UiName { get; internal set; }
    public ClassDef Ui { get; internal set; }

    public override string ToString() {
      return string.Format("{0}.{1}", Owner.Name, Name);
    }
  }
}

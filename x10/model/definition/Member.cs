using System;
using System.Collections.Generic;
using System.Text;

namespace x10.model.definition {
  public class Member : ModelComponent {
    public bool IsMandatory { get; set; }
    public bool IsReadOnly { get; set; }
  }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace x10.model.definition {

  public class Validation : ModelComponent {
    public string Message { get; set; }
    public string Trigger { get; set; }
    public Entity Owner { get; set; }

    public Validation() {
      // Do nothing
    }
  }
}

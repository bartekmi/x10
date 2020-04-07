using System;
using System.Collections.Generic;
using System.Text;

using x10.model.definition;

namespace x10.ui.composition {
  public class InstanceModelRef : Instance {
    public override string ToString() {
      return string.Format("Model Reference @ {0}. Member = {1}", Path, ModelMember?.Name);
    }
  }
}

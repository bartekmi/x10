using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using x10.model.definition;

namespace x10.ui.composition {
  public class InstanceClassDefUse : Instance {

    public override string ToString() {
      return string.Format("Component Use @ {0}. Member = {1}", Path, ModelMember?.Name);
    }
  }
}

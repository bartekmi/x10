using System;
using System.Collections.Generic;
using System.Text;
using x10.model.definition;

namespace x10.ui.composition {
  public class InstanceClassDefUse : Instance {
    // Nested Ui child components (either Component Use or model references)
    public List<Instance> Children { get; private set; }

    public InstanceClassDefUse() {
      Children = new List<Instance>();
    }

    internal void AddChild(Instance instance) {
      Children.Add(instance);
      // Likely, in the future, we'll need to keep track of parent. Do it here.
    }

    public override string ToString() {
      return string.Format("Component Use @ {0}. Member = {1}", Path, ModelMember?.Name);
    }
  }
}

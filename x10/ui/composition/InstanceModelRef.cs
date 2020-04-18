using System;
using System.Collections.Generic;

using x10.parsing;

namespace x10.ui.composition {
  public class InstanceModelRef : Instance {

    public InstanceModelRef(XmlElement xmlElement, UiAttributeValue owner) : base(xmlElement, owner) {
      // Do nothing
    }

    public override string GetElementName() {
      return Path;
    }

    public override string ToString() {
      return string.Format("Model Reference @ {0}. Member = {1}", Path, ModelMember?.Name);
    }
  }
}

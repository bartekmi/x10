using System;
using System.Collections.Generic;

using x10.parsing;

namespace x10.ui.composition {
  // This type of Instance is created from a lower-case attribute mention in the XML file
  // - e.g. <name/>
  // Here, the RenderAs property has to be calculated by looking first
  // at the refe
  public class InstanceModelRef : Instance {

    public InstanceModelRef(XmlElement xmlElement, UiAttributeValue owner) : base(xmlElement, owner) {
      // Do nothing
    }

    public override string DebugPrintAs() {
      return Path;
    }

    public override string ToString() {
      return string.Format("Model Reference @ {0}. Member = {1}", Path, ModelMember?.Name);
    }
  }
}

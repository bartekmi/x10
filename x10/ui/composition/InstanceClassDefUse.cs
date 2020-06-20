using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using x10.parsing;
using x10.ui.metadata;

namespace x10.ui.composition {
  // TODO: Explain this!
  public class InstanceClassDefUse : Instance {
    public InstanceClassDefUse(ClassDef classDef, XmlElement xmlElement, UiAttributeValue owner) : base(xmlElement, owner) {
      RenderAs = classDef;
    }

    public override string GetElementName() {
      return RenderAs.Name;
    }

    public override string ToString() {
      return string.Format("ClassDef Use of {0}. Path: {1}. Member: {2}", RenderAs.Name, Path, ModelMember?.Name);
    }
  }
}

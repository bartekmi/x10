using System;
using System.Collections.Generic;
using System.Text;

using x10.parsing;
using x10.ui.metadata;

namespace x10.ui.composition {
  public abstract class UiAttributeValue {
    public UiAttributeDefinition Definition { get; set; }

    public XmlBase XmlBase { get; set; }

    public UiAttributeValue(XmlBase xmlBase) {
      XmlBase = xmlBase;
    }
  }
}

using System;
using System.Collections.Generic;
using System.Text;

using x10.parsing;
using x10.ui.metadata;

namespace x10.ui.composition {
  public abstract class UiAttributeValue {
    public UiAttributeDefinition Definition { get; private set; }
    public IAcceptsUiAttributeValues Owner { get; private set; }
    public XmlBase XmlBase { get; private set; }

    protected UiAttributeValue(UiAttributeDefinition attrDefinition, IAcceptsUiAttributeValues owner, XmlBase xmlBase) {
      Definition = attrDefinition;
      Owner = owner;
      XmlBase = xmlBase;
    }
  }
}

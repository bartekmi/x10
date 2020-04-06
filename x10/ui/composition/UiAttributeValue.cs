using System;
using System.Collections.Generic;
using System.Text;

using x10.parsing;
using x10.ui.metadata;

namespace x10.ui.composition {
  public class UiAttributeValue {
    public UiAttributeDefinition Definition { get; set; }
    public object Value { get;set; }

    public XmlScalar XmlScalar { get; set; }

    public UiAttributeValue(XmlScalar xmlScalar) {
      XmlScalar = xmlScalar;
    }

    public override string ToString() {
      return string.Format("UI Attribute Value: {0}", Value);
    }
  }
}

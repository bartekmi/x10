using System;
using System.Collections.Generic;
using System.Text;

using x10.parsing;
using x10.ui.metadata;

namespace x10.ui.composition {
  public class UiAttributeValueAtomic : UiAttributeValue {
    public object Value { get;set; }

    public UiAttributeValueAtomic(XmlScalar xmlScalar) : base(xmlScalar) {
      // Do nothing
    }

    public override string ToString() {
      return string.Format("Atomic Value: {0}", Value);
    }
  }
}

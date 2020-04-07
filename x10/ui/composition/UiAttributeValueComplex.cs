using System;
using System.Collections.Generic;
using System.Text;

using x10.parsing;
using x10.ui.metadata;

namespace x10.ui.composition {
  public class UiAttributeValueComplex : UiAttributeValue {
    public List<Instance> Instances { get; set; }

    public UiAttributeValueComplex(XmlElement xmlElement) : base(xmlElement) {
      // To nothing
    }

    public override string ToString() {
      return string.Format("Complex Value: for attribute {0}", Definition.Name);
    }
  }
}

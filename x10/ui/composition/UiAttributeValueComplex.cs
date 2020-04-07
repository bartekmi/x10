using System;
using System.Collections.Generic;
using System.Text;

using x10.parsing;
using x10.ui.metadata;

namespace x10.ui.composition {
  public class UiAttributeValueComplex : UiAttributeValue {
    public List<Instance> Instances { get; private set; }

    public UiAttributeValueComplex(XmlBase xmlBase) : base(xmlBase) {
      Instances = new List<Instance>();
    }

    public void AddInstance(Instance instance) {
      Instances.Add(instance);
    }

    public override string ToString() {
      return string.Format("Complex Value: for attribute {0}", Definition.Name);
    }
  }
}

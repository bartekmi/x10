using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using x10.utils;
using x10.parsing;
using x10.ui.metadata;
using System.Linq;

namespace x10.ui.composition {
  public class ClassDefX10 : ClassDef, IAcceptsUiAttributeValues {
    public Instance RootChild { get; set; }

    // IAcceptsUiAttributeValues
    public List<UiAttributeValue> AttributeValues { get; private set; }
    public XmlElement XmlElement { get; set; }
    public ClassDef ClassDef { get { return this; } }
    public string DebugPrintAs() {
      return Name;
    }

    public ClassDefX10(XmlElement xmlRoot) {
      XmlElement = xmlRoot;
      AttributeValues = new List<UiAttributeValue>();
    }
  }
}

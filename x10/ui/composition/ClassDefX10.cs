using System;
using System.Collections.Generic;
using System.Text;
using x10.parsing;
using x10.ui.metadata;

namespace x10.ui.composition {
  public class ClassDefX10 : ClassDef, IAcceptsUiAttributeValues {
    public Instance RootChild { get; set; }
    public List<UiAttributeValue> AttributeValues { get; private set; }
    public XmlBase XmlElement { get; set; }

    public ClassDefX10() {
      AttributeValues = new List<UiAttributeValue>();
    }
  }
}

using System;
using System.Collections.Generic;
using System.Text;
using x10.parsing;
using x10.ui.metadata;

namespace x10.ui.composition {
  public class UiDefinitionX10 : UiDefinition, IAcceptsUiAttributeValues {
    public UiChild RootChild { get; set; }
    public List<UiAttributeValue> AttributeValues { get; private set; }
    public XmlBase XmlElement { get; set; }

    public UiDefinitionX10() {
      AttributeValues = new List<UiAttributeValue>();
    }
  }
}

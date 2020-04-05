using System;
using System.Collections.Generic;
using System.Text;
using x10.ui.metadata;

namespace x10.ui.composition {
  public abstract class UiChild {
    // Every UI child eventually must resolve to a visual (UI) component
    public UiDefinition RenderedAs {get; set; }

    // Attribute values of this UiChild
    public List<UiAttributeValue> AttributeValues { get; set; }
  }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using x10.parsing;
using x10.model.definition;
using x10.ui.metadata;

namespace x10.ui.composition {
  public abstract class Instance : IAcceptsUiAttributeValues {
    // The Entity Member which is being displayed/edited by this UiChild
    // If null, this is the root child, or one of its consecutive children
    // where no path has been set yet.
    public Member ModelMember { get; set; }

    // This is analogous to Path in WPF... it is a dot-separated list of
    // model members descending down the graph of model objects
    // Note that for InstanceModelReference, this is simply the name of the Xml element
    public string Path { get; set; }

    // Every UI child eventually must resolve to a visual (UI) component
    public ClassDef RenderAs { get; set; }

    public List<UiAttributeValue> AttributeValues { get; private set; }
    public XmlBase XmlElement { get; set; }


    // Constructor
    protected Instance() {
      AttributeValues = new List<UiAttributeValue>();
    }

    public UiAttributeValue PrimaryValue {
      get { return AttributeValues.SingleOrDefault(x => x.Definition.IsPrimary); }
    }
  }
}

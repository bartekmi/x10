using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

using x10.utils;
using x10.parsing;
using x10.model.definition;
using x10.ui.metadata;

namespace x10.ui.composition {
  // Where as a ClassDef defines (typically) a UI component in terms
  // of its name and attributes - and hence is analogous to a C# class
  // - Instance is an "instantiation" of that component - with specific
  // values for all the attributes (expressed as UiAttributeValue's)
  public abstract class Instance : IAcceptsUiAttributeValues {

    public abstract string GetElementName();

    // The Entity Member which is being displayed/edited by this Instance
    // If null, this is the root child, or one of its consecutive children
    // where no path has been set yet.
    public Member ModelMember { get; set; }

    // This is analogous to Path in WPF... it is a dot-separated list of
    // model members descending down the graph of model objects
    // Note that for InstanceModelReference, this is simply the name of the Xml element
    public string Path { get; set; }

    // Every UI child eventually must resolve to a visual (UI) component
    public ClassDef RenderAs { get; set; }

    // All but the root-level Instance are owned by a UiAttributeValue
    public UiAttributeValue Owner { get; private set; }

    // IAcceptsUiAttributeValues
    public List<UiAttributeValue> AttributeValues { get; private set; }
    public XmlElement XmlElement { get; private set; }
    public ClassDef ClassDef { get { return RenderAs; } }

    // Derived
    public Instance Parent { 
      get { return Owner?.Owner as Instance; }
    }

    // Constructor
    protected Instance(XmlElement xmlElement, UiAttributeValue owner) {
      XmlElement = xmlElement;
      Owner = owner;
      AttributeValues = new List<UiAttributeValue>();
    }

    public UiAttributeValue PrimaryValue {
      get { return AttributeValues.SingleOrDefault(x => x.Definition.IsPrimary); }
    }
  }
}

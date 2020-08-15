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

    // Currently, this is only used for printing - i.e. debug
    // and unit test purposes
    public abstract string DebugPrintAs();

    // The Entity Member which is being displayed/edited by this Instance
    // If null, this is the root child, or one of its consecutive children
    // where no path has been set yet.
    public Member ModelMember { get; set; }
    // public Member ModelMember { get { return PathComponents == null ? null : PathComponents.LastOrDefault(); } }

    // The path components of this Instance. 
    public List<Member> PathComponents { get; set; }

    // This is analogous to Path in WPF... it is a dot-separated list of
    // model members descending down the graph of model objects
    // Note that for InstanceModelReference, this is simply the name of the Xml element
    public string Path { get; set; }

    // Every UI child eventually must resolve to a visual (UI) component
    public ClassDef RenderAs { get; set; }

    // All but the root-level Instance are owned by a UiAttributeValueComplex
    public UiAttributeValueComplex Owner { get; private set; }

    // If true, this Instance was inserted as a wrapper around an InstanceModelRef
    public bool IsWrapper { get; set; }

    // IAcceptsUiAttributeValues
    public List<UiAttributeValue> AttributeValues { get; private set; }
    public XmlElement XmlElement { get; private set; }
    public ClassDef ClassDef { get { return RenderAs; } }

    // Derived
    public Instance ParentInstance { get { return Owner?.Owner as Instance; } }
    public IEnumerable<Instance> ChildInstances {
      get {
        return AttributeValues.OfType<UiAttributeValueComplex>().SelectMany(x => x.Instances);
      }
    }

    // Constructor
    protected Instance(XmlElement xmlElement, UiAttributeValueComplex owner) {
      XmlElement = xmlElement;
      Owner = owner;
      AttributeValues = new List<UiAttributeValue>();
    }

    public UiAttributeValue PrimaryValue {
      get { return AttributeValues.SingleOrDefault(x => x.Definition.IsPrimary); }
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;

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

    // Instance id for any purpose - e.g. debugging. You can set the id property on any 
    // UI component and trigger a conditional break-point on that id anywhere in the code
    public string Id {get; set;}

    // The Entity Member which is being displayed/edited by this Instance
    // If null, this is the root child, or one of its consecutive children
    // where no path has been set yet.
    public Member ModelMember { get; set; }

    // The path components of this Instance. 
    public List<Member> PathComponents { get; set; }

    // This is analogous to Path in WPF... it is a dot-separated list of
    // model members descending down the graph of model objects
    // Note that for InstanceModelReference, this is simply the name of the Xml element
    public string Path { get; set; }

    // Every UI child eventually must resolve to a visual (UI) component
    public ClassDef RenderAs { get; set; }

    // All but the root-level Instance are owned by a UiAttributeValueComplex
    public UiAttributeValueComplex Owner { get; set; }

    // If true, this Instance was inserted as a wrapper around an InstanceModelRef
    public bool IsWrapper { get; set; }

    // IAcceptsUiAttributeValues
    public List<UiAttributeValue> AttributeValues { get; private set; }
    public XmlElement XmlElement { get; private set; }
    public ClassDef ClassDef { get { return RenderAs; } }

    // Unlike the similar version defined as extension methods on IAcceptsUiAttributeValues,
    // this method respects the "Inheritable" property of UiAttributeDefinitionAtomic
    public UiAttributeValueAtomic FindAttributeValueRespectInheritable(UiAttributeDefinitionAtomic definition) {
      Instance instance = this;

      // Traverse the instances, going up the parent chain
      while (instance != null) {
        // Some logical components define "hard-coded" attached attributes. Classic case is that the standard Table
        // component is always read-only
        UiAttributeValueAtomic value 
          = instance.RenderAs.FindDefaultAttachedAttribute(definition) as UiAttributeValueAtomic;

        // Attempt to find the value in the current instance
        if (value == null)
          value = instance.AtomicAttributeValues().SingleOrDefault(x => x.Definition == definition);

        if (value != null || !definition.IsInheritable)
          return value;
        instance = instance.ParentInstance;
      }

      return null;
    }

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

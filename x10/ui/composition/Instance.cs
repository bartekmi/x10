﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

using x10.utils;
using x10.parsing;
using x10.model.definition;
using x10.ui.metadata;

namespace x10.ui.composition {
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

    public List<UiAttributeValue> AttributeValues { get; private set; }
    public XmlBase XmlElement { get; private set; }

    // Derived
    public IEnumerable<UiAttributeValueAtomic> AtomicAttributeValues {
      get { return AttributeValues.OfType<UiAttributeValueAtomic>(); }
    }
    public IEnumerable<UiAttributeValueComplex> ComplexAttributeValues {
      get { return AttributeValues.OfType<UiAttributeValueComplex>(); }
    }

    // Constructor
    protected Instance(XmlElement xmlElement) {
      XmlElement = xmlElement;
      AttributeValues = new List<UiAttributeValue>();
    }

    public UiAttributeValue PrimaryValue {
      get { return AttributeValues.SingleOrDefault(x => x.Definition.IsPrimary); }
    }

    public void Print(TextWriter writer, int indent, PrintConfig config = null) {
      PrintUtils.Indent(writer, indent);
      writer.Write("<" + GetElementName());

      foreach (UiAttributeValueAtomic atomic in AtomicAttributeValues) 
        atomic.Print(writer);

      if (config != null && config.AlwaysPrintPath)
        if (!AtomicAttributeValues.Any(x => x.Definition.Name == "path"))
          writer.Write(" path='{0}'", Path);

      if (ComplexAttributeValues.Count() == 0)
        writer.WriteLine("/>");
      else {
        writer.WriteLine(">");

        foreach (UiAttributeValueComplex complex in ComplexAttributeValues)
          complex.Print(writer, indent + 1, config);

        PrintUtils.Indent(writer, indent);
        writer.WriteLine("</" + GetElementName() + ">");
      }
    }
  }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using x10.parsing;
using x10.ui.metadata;
using x10.utils;

namespace x10.ui.composition {
  public class UiAttributeValueComplex : UiAttributeValue {
    public List<Instance> Instances { get; private set; }

    // Derived
    public UiAttributeDefinitionComplex DefinitionComplex { get { return (UiAttributeDefinitionComplex)Definition; } }

    public UiAttributeValueComplex(UiAttributeDefinitionComplex attrDefinition, IAcceptsUiAttributeValues owner, XmlBase xmlBase) 
      : base(attrDefinition, owner, xmlBase) {
      Instances = new List<Instance>();
    }

    public void AddInstance(Instance instance) {
      instance.Owner = this;
      Instances.Add(instance);
    }

    // Replace old instance with _new. Old instance MUST exist;
    public void ReplaceInstance(Instance old, Instance _new) {
      int index = Instances.IndexOf(old);
      if (index == -1)
        throw new Exception("Old instance not found");
      _new.Owner = this;
      Instances[index] = _new;
    }

    public void Print(TextWriter writer, int indent, PrintConfig config = null) {
      if (Definition.IsPrimary)
        PrintValues(writer, indent, config);
      else {
        PrintUtils.Indent(writer, indent);
        writer.WriteLine("<{0}.{1}>", Owner.DebugPrintAs(), Definition.Name);
        PrintValues(writer, indent + 1, config);
        PrintUtils.Indent(writer, indent);
        writer.WriteLine("</{0}.{1}>", Owner.DebugPrintAs(), Definition.Name);
      }
    }

    private void PrintValues(TextWriter writer, int indent, PrintConfig config = null) {
      foreach (Instance instance in Instances)
        instance.Print(writer, indent, config);
    }


    public override string ToString() {
      return string.Format("{0}=[{1} instances]", Definition?.Name, Instances.Count);
    }
  }
}

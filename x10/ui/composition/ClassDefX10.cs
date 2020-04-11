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
    public List<UiAttributeValue> AttributeValues { get; private set; }
    public XmlBase XmlElement { get; set; }

    public ClassDefX10(XmlElement xmlRoot) {
      XmlElement = xmlRoot;
      AttributeValues = new List<UiAttributeValue>();
    }

    public void Print(TextWriter writer, int indent, PrintConfig config = null) {
      PrintUtils.Indent(writer, indent);
      writer.Write("<" + Name);

      foreach (UiAttributeValueAtomic atomic in AttributeValues.Cast<UiAttributeValueAtomic>()) 
        atomic.Print(writer);

      writer.WriteLine(">");

      if (RootChild != null)
        RootChild.Print(writer, indent + 1, config);

      PrintUtils.Indent(writer, indent);
      writer.WriteLine("</" + Name + ">");
    }
  }
}

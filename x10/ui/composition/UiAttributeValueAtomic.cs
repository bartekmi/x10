using System.IO;

using x10.formula;
using x10.parsing;
using x10.ui.metadata;

namespace x10.ui.composition {
  public class UiAttributeValueAtomic : UiAttributeValue {
    public object Value { get; set; }
    public string Formula { get; set; }
    public ExpBase Expression { get; set; }

    // Derived
    public UiAttributeDefinitionAtomic DefinitionAtomic { get { return (UiAttributeDefinitionAtomic)Definition; } }
    public Instance Instance { get { return (Instance)Owner; } }

    public UiAttributeValueAtomic(UiAttributeDefinitionAtomic attrDefinition, IAcceptsUiAttributeValues owner, XmlBase xmlBase)
      : base(attrDefinition, owner, xmlBase) {
      // Do nothing
    }

    public void Print(TextWriter writer) {
      if (Definition.Name == ParserXml.ELEMENT_NAME)
        return;

      UiAttributeDefinitionAtomic attrDef = (UiAttributeDefinitionAtomic)Definition;

      writer.Write(" ");
      writer.Write("{0}{1}='{2}'",
        attrDef.IsAttached ? attrDef.Owner.Name + "." : null,
        Definition.Name, 
        Formula == null ? Value : "=" + Formula);  // Equals is stripped during compilation
    }

    public override string ToString() {
      if (Formula != null)
        return string.Format("{0} =>'{1}'", Definition?.Name, Formula);
      return string.Format("{0}='{1}'", Definition?.Name, Value);
    }
  }
}

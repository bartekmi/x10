using System.Collections.Generic;
using System.Linq;

using x10.model;
using x10.parsing;
using x10.ui.metadata;
using static x10.ui.metadata.ClassDefNative;

namespace x10.ui.composition {
  // Each ClassDefX10 is define by its own XML file, represents a composite
  // UX component, and may be included/embedded in another ClassDefX10.
  // The actual content is specified by an Instance in RootChild.
  public class ClassDefX10 : ClassDef, IAcceptsUiAttributeValues {
    public Instance RootChild { get; set; }
    public string Url { get; set; }

    // IAcceptsUiAttributeValues
    public List<UiAttributeValue> AttributeValues { get; private set; }
    public XmlElement XmlElement { get; set; }
    public ClassDef ClassDef { get { return this; } }
    public string DebugPrintAs() {
      return Name;
    }

    public ClassDefX10(XmlElement xmlRoot) {
      XmlElement = xmlRoot;
      AttributeValues = new List<UiAttributeValue>();
    }

    // Returns state information by parsing the state complex attribute.
    // Returns null if there is no state.
    public IEnumerable<StateClass> GetStateVariables(AllEntities allEntities, AllEnums allEnums) {
      UiAttributeValueComplex states = this.FindAttributeValue(STATE_ATTRIBUTE) as UiAttributeValueComplex;
      if (states == null)
        return null;

      return states.Instances.Select(x => StateClass.FromInstance(allEntities, allEnums, x));
    }
  }
}

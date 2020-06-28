using System.Collections.Generic;
using System.Linq;

using x10.model;
using x10.parsing;
using x10.ui.metadata;
using x10.ui.composition;
using static x10.ui.metadata.ClassDefNative;

namespace x10.ui.composition {
  public class ClassDefX10 : ClassDef, IAcceptsUiAttributeValues {
    public Instance RootChild { get; set; }

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
    public IEnumerable<StateClass> GetStateVariables(AllEnums allEnums) {
      UiAttributeValueComplex states = this.FindAttributeValue(ClassDefNative.STATE) as UiAttributeValueComplex;
      if (states == null)
        return null;

      return states.Instances.Select(x => StateClass.FromInstance(allEnums, x));
    }
  }
}

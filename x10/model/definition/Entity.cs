using System;
using System.Collections.Generic;
using System.Linq;

namespace x10.model.definition {
  public class Entity : ModelComponent{
    public Entity InheritsFrom { get; set; }
    public List<Member> Members { get; private set; }

    // Derived
    public IEnumerable<X10Attribute> Attributes { get { return Members.OfType<X10Attribute>();  } }
    public IEnumerable<Association> Associations { get { return Members.OfType<Association>(); } }

    public Entity() {
      Members = new List<Member>();
    }
  }
}

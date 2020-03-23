using System;
using System.Collections.Generic;
using System.Text;

namespace x10.model.definition {
  public class Entity : ModelComponent{
    public Entity InheritsFrom { get; set; }
    public List<Member> Members { get; private set; }

    public Entity() {
      Members = new List<Member>();
    }
  }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace x10.model.definition {
  public class Entity : ModelComponent{
    public String InheritsFromName { get; set; }
    public List<Member> Members { get; private set; }

    // Derived
    public IEnumerable<X10Attribute> Attributes { get { return Members.OfType<X10Attribute>(); } }
    public IEnumerable<X10RegularAttribute> RegularAttributes { get { return Members.OfType<X10RegularAttribute>(); } }
    public IEnumerable<X10DerivedAttribute> DrivedAttributes { get { return Members.OfType<X10DerivedAttribute>(); } }
    public IEnumerable<Association> Associations { get { return Members.OfType<Association>(); } }

    // Rehydrated
    public Entity InheritsFrom { get; set; }

    public Entity() {
      Members = new List<Member>();
    }
  }
}

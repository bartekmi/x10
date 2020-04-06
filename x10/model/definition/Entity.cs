using System;
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

    public Member FindMemberByName(string name) {
      // TODO: Consider making this similar to the other 'All...' classes where an appropriate
      // message is generated if member is missing or duplicated
      // Current implementatino will give mis-leading results if two members are present.
      return Members.FirstOrDefault(x => x.Name == name);
    }

    internal void AddMembers(Member member) {
      member.Owner = this;
      Members.Add(member);
    }
  }
}

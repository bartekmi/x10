using System;
using System.Collections.Generic;
using System.Linq;
using x10.ui.metadata;

namespace x10.model.definition {
  public class Entity : ModelComponent{

    private List<Member> _localMembers = new List<Member>();
    public List<Member> LocalMembers {
      get { return _localMembers; }
      set {
        foreach (Member member in value)
          AddMembers(member);
      }
    }

    public String InheritsFromName { get; set; }
    public string UiName { get; set; }
    public bool IsAbstract { get; set; }

    // Derived
    public IEnumerable<Member> Members {
      get {
        return InheritsFrom == null ?
          LocalMembers :
          InheritsFrom.Members.Concat(LocalMembers);
      }
    }

    public IEnumerable<X10Attribute> Attributes { get { return Members.OfType<X10Attribute>(); } }
    public IEnumerable<X10RegularAttribute> RegularAttributes { get { return Members.OfType<X10RegularAttribute>(); } }
    public IEnumerable<X10DerivedAttribute> DerivedAttributes { get { return Members.OfType<X10DerivedAttribute>(); } }
    public IEnumerable<Association> Associations { get { return Members.OfType<Association>(); } }
    public IEnumerable<Member> EditableMembers { get { return Members.Where(x => !x.IsReadOnly); } }

    // Rehydrated
    public Entity InheritsFrom { get; internal set; }
    public ClassDef Ui { get; internal set; }

    public Entity() {
      InheritsFrom = Object;    // May be explicitly overridden in definition
    }

    // Is-a in an object-oriented sense. Returns true if the passed in parameter is this Entity
    // or if this Entity is a descndent of classDefOrAncestor
    public bool IsA(Entity entityOrAncestor) {
      Entity entity = this;
      while (entity != null) {
        if (entity == entityOrAncestor)
          return true;
        entity = entity.InheritsFrom;
      }
      return false;
    }

    public Member FindMemberByName(string name) {
      // TODO: Consider making this similar to the other 'All...' classes where an appropriate
      // message is generated if member is missing or duplicated
      // Current implementatino will give mis-leading results if two members are present.
      return Members.FirstOrDefault(x => x.Name == name);
    }

    internal void AddMembers(Member member) {
      member.Owner = this;
      LocalMembers.Add(member);
    }

    #region Primordial Entities
    public static Entity Object = new Entity() {
      Name = "EntityObject",
    };
    #endregion
  }
}

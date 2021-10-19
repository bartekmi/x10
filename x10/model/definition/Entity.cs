using System;
using System.Collections.Generic;
using System.Linq;

using x10.ui.metadata;
using x10.formula;

namespace x10.model.definition {
  public class Entity : ModelComponent{

    private List<Member> _localMembers = new List<Member>();
    public List<Member> LocalMembers {
      get { return _localMembers; }
      set {
        foreach (Member member in value)
          AddMember(member);
      }
    }
    public List<Validation> Validations { get; } = new List<Validation>();

    public String InheritsFromName { get; set; }
    public string UiName { get; set; }
    public bool IsAbstract { get; set; }

    // If true, this entity represents something that is NOT fetched from the database.
    // Examples include "Entities" associated with DataType for things like year/month/etc of DateTime.
    public bool IsNonFetchable {get;set;}

    public ExpBase StringRepresentation { get; set; }

    // Derived
    public IEnumerable<Member> Members {
      get {
        return InheritsFrom == null ?
          LocalMembers :
          InheritsFrom.Members.Concat(LocalMembers);
      }
    }
    public bool IsContext => Name == ModelValidationUtils.CONTEXT_ENTITY_NAME;

    public IEnumerable<X10Attribute> Attributes { get { return Members.OfType<X10Attribute>(); } }
    public IEnumerable<X10RegularAttribute> RegularAttributes { get { return Members.OfType<X10RegularAttribute>(); } }
    public IEnumerable<X10DerivedAttribute> DerivedAttributes { get { return Members.OfType<X10DerivedAttribute>(); } }
    public IEnumerable<Association> Associations { get { return Members.OfType<Association>(); } }

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

    internal void AddMember(Member member) {
      member.Owner = this;
      LocalMembers.Add(member);
    }

    internal void AddValidation(Validation validation) {
      validation.Owner = this;
      Validations.Add(validation);
    }

    #region Primordial Entities
    public static Entity Object = new Entity() {
      Name = "EntityObject",
    };
    #endregion
  }
}

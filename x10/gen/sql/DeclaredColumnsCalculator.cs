using System;
using System.Collections.Generic;
using System.Linq;
using x10.gen.sql.primitives;
using x10.model;
using x10.model.definition;

namespace x10.gen.sql {
  internal class DeclaredColumnsCalculator {

    private Dictionary<Entity, List<MemberAndOwner>> _reverseAssociations;
    private IEnumerable<Entity> _realEntities;

    // Derived
    internal IEnumerable<MemberAndOwner> AllForward {
      get {
        return GetRealEntities().SelectMany(x => GetForwardAssociations(x));
      }
    }
    internal IEnumerable<MemberAndOwner> AllReverse {
      get {
        return GetRealEntities().SelectMany(x => GetReverseAssociations(x));
      }
    }
    internal IEnumerable<MemberAndOwner> AllAssociations {
      get {
        return AllForward.Concat(AllReverse);
      }
    }

    internal DeclaredColumnsCalculator(IEnumerable<Entity> entities) {
      _realEntities = entities.Where(x => !Ignore(x));
      List<MemberAndOwner> reverseOwners = new List<MemberAndOwner>();

      foreach (Entity entity in _realEntities)
        foreach (Association association in entity.Associations.Where(x => IsReverse(x) && !IgnoreColumn(x)))
          reverseOwners.Add(new MemberAndOwner(ColumnType.ReverseAssociation, association, entity));

      _reverseAssociations = reverseOwners
        .GroupBy(ro => ro.Association.ReferencedEntity)
        .ToDictionary(grp => grp.Key, grp => grp.ToList());
    }

    internal IEnumerable<Entity> GetRealEntities() {
      return _realEntities;
    }

    private static bool Ignore(Entity entity) {
      return entity.FindBoolean(DataGenLibrary.NO_SQL_SCHEMA, false) ||
        entity.IsAbstract ||
        entity.IsContext;
    }

    internal IEnumerable<MemberAndOwner> GetDeclaredColumns(Entity entity) {
      return entity.Attributes.Select(x => new MemberAndOwner(ColumnType.Attribute, x))   // All attributes
        .Concat(GetForwardAssociations(entity))                                           // Forward assoc's
        .Concat(GetReverseAssociations(entity));                                          // Reverse assoc's
    }

    internal IEnumerable<MemberAndOwner> GetForwardAssociations(Entity entity) {
      return entity.Associations
        .Where(x => !IsReverse(x) && !HasCorrespondingReverseAssociation(x) && !IgnoreColumn(x))
        .Select(x => new MemberAndOwner(ColumnType.ForwardAssociation, x, entity));
    }

    internal List<MemberAndOwner> GetReverseAssociations(Entity entity) {
      if (_reverseAssociations.TryGetValue(entity, out List<MemberAndOwner> associations))
        return associations;
      return new List<MemberAndOwner>();
    }

    // Capture the logic in a single place whether an association is owned by defining entity
    // or the entity that it points to.
    // For multiple associations, this is obvious, but for single associations, we actually
    // have a choice, since the Foreign Key (FK) could point in either direction.
    // We choose the simpler (though potentially less performant) options where ALL owned
    // associations point from child to parent.
    internal static bool IsReverse(Association association) {
      return association.Owns;
    }

    // TODO: This is a bit of a hack until we introduce derived associations
    // Put here for the sake of the various ShipmentLocation associations from Shipment
    internal static bool IgnoreColumn(Association association) {
      return association.FindValue<SqlRange>(DataGenLibrary.QUANTITY, out SqlRange quantity) && quantity.IsZero;
    }

    private bool HasCorrespondingReverseAssociation(Association association) {
      List<MemberAndOwner> reverseAssociations = GetReverseAssociations(association.Owner);
      return reverseAssociations.Any(x => x.ActualOwner == association.ReferencedEntity);
    }
  }

  #region Helper Classes

  internal enum ColumnType {
    Attribute,
    ForwardAssociation,
    ReverseAssociation,
  }

  // It seems counter-intuitive that we can't just use the Association objects to keep track of its
  // actual owner. 
  // But there is an issue with shared Associations inherited from the same base class
  // by multiple Entities. For all of these, the "Owner" points to the base class, so we can't identify
  // who *really* owns the Association
  internal class MemberAndOwner {
    internal ColumnType Type { get; private set; }
    internal Member Member { get; private set; }
    internal Entity ActualOwner { get; private set; }

    // Derived
    internal Association Association { get { return (Association)Member; } }
    internal X10Attribute Attribute { get { return (X10Attribute)Member; } }

    internal MemberAndOwner(ColumnType type, Member member, Entity owner = null) {
      Type = type;
      Member = member;
      ActualOwner = owner ?? member.Owner;
    }

    public override string ToString() {
      return string.Format("{0}: {1} - {2}", Type, ActualOwner.Name, Member);
    }
  }
  #endregion
}

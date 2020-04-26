using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using x10.model.definition;

namespace x10.gen.sql {
  internal class ReverseAssociationCalculator {

    private Dictionary<Entity, List<ReverseOwner>> _reverseAssociations;

    internal ReverseAssociationCalculator(IEnumerable<Entity> entities) {
      List<ReverseOwner> reverseOwners = new List<ReverseOwner>();
      foreach (Entity entity in entities) 
        foreach (Association association in entity.Associations.Where(x => x.IsMany)) 
          reverseOwners.Add(new ReverseOwner(entity, association));

      _reverseAssociations = reverseOwners
        .GroupBy(ro => ro.Assoc.ReferencedEntity)
        .ToDictionary(grp => grp.Key, grp => grp.ToList());
    }

    internal List<ReverseOwner> Get(Entity entity) {
      if (_reverseAssociations.TryGetValue(entity, out List<ReverseOwner> associations))
        return associations;
      return new List<ReverseOwner>();
    }
  }

  #region Helper Classes

  // It seems counter-intuitive that we just can use the Association objects to keep track of the
  // Reverse references. And so thought I, until...
  // I realized that there is an issue with shared Associations inherited from the same base class
  // by multiple Entities. For all of these, the "Owner" points to the base class, so we can't identify
  // who *really* owns the Association
  internal class ReverseOwner {
    internal Entity ActualOwner { get; private set; }
    internal Association Assoc { get; private set; }

    internal ReverseOwner(Entity owner, Association association) {
      ActualOwner = owner;
      Assoc = association;
    }
  }
  #endregion
}

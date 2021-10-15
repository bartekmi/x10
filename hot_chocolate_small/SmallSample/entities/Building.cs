using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.SmallSample.Repositories;

namespace x10.hotchoc.SmallSample.Entities {
  // Enums
  public enum MailboxTypeEnum {
    InBuilding,
    Community,
    Individual,
  }

  public enum PetPolicyEnum {
    NoPets,
    AllPetsOk,
    CatsOnly,
    DogsOnly,
  }


  /// <summary>
  /// A physical building which contains rental units
  /// </summary>
  public class Building : Base {
    // Regular Attributes
    [GraphQLNonNullType]
    public string? Moniker { get; set; }
    [GraphQLNonNullType]
    public string? Name { get; set; }
    [GraphQLNonNullType]
    public string? Description { get; set; }
    [GraphQLNonNullType]
    public DateTime? DateOfOccupancy { get; set; }
    [GraphQLNonNullType]
    public MailboxTypeEnum? MailboxType { get; set; }
    public PetPolicyEnum? PetPolicy { get; set; }
    [GraphQLNonNullType]
    public bool MailingAddressSameAsPhysical { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return Name; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    // Associations
    [GraphQLNonNullType]
    public List<Unit>? Units { get; set; }
    [GraphQLNonNullType]
    public Address? PhysicalAddress { get; set; }
    public Address? MailingAddress { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
      Units?.ForEach(x => x.EnsureUniqueDbid());
      PhysicalAddress?.EnsureUniqueDbid();
      MailingAddress?.EnsureUniqueDbid();
    }

    internal override void SetNonOwnedAssociations(IRepository repository) {
      base.SetNonOwnedAssociations(repository);

      if (Units != null)
        foreach (Unit units in Units)
          units.SetNonOwnedAssociations(repository);

      PhysicalAddress?.SetNonOwnedAssociations(repository);

      MailingAddress?.SetNonOwnedAssociations(repository);
    }
  }
}


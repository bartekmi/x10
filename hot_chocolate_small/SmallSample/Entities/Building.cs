using System;
using System.Collections.Generic;

using HotChocolate;

namespace Small.Entities {

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
  /// A physical structure that contains Units to be occupied by Tenants
  /// </summary>
  public class Building : EntityBase {

    // Regular Attributes
    public string? Moniker { get; set; }
    [GraphQLNonNullType]
    public string? Name { get; set; }
    [GraphQLNonNullType]
    public string? Description { get; set; }
    public DateTime? DateOfOccupancy { get; set; }
    public MailboxTypeEnum? MailboxType { get; set; }
    public PetPolicyEnum? PetPolicy { get; set; }
    public bool MailingAddressSameAsPhysical { get; set; }

    // Associations
    public List<Unit>? Units { get; set; }
    public Address? PhysicalAddress { get; set; }
    public Address? MailingAddress { get; set; }
  }
}

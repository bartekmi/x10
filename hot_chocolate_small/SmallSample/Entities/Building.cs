using System;
using System.Collections.Generic;

using HotChocolate;

namespace x10.hotchoc.Entities {
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
    public string? ToStringRepresentation => Name;

    // Associations
    [GraphQLNonNullType]
    public List<Unit>? Units { get; set; }
    [GraphQLNonNullType]
    public Address? PhysicalAddress { get; set; }
    public Address? MailingAddress { get; set; }

  }
}


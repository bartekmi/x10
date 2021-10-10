using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.dps.Repositories;

namespace x10.hotchoc.dps.Entities {
  /// <summary>
  /// A portion of Core CompanyEntity entity
  /// </summary>
  public class CompanyEntity : Base {
    // Regular Attributes
    public int? ClientId { get; set; }
    [GraphQLNonNullType]
    public string? Name { get; set; }
    [GraphQLNonNullType]
    public string? PrimaryContact { get; set; }
    [GraphQLNonNullType]
    public string? PrimaryContactEmail { get; set; }
    [GraphQLNonNullType]
    public string? MainNumber { get; set; }
    [GraphQLNonNullType]
    public string? Segment { get; set; }
    [GraphQLNonNullType]
    public string? Website { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return "CompanyEntity: " + Dbid; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    // Associations
    [GraphQLNonNullType]
    public AddressType? PhysicalAddress { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
      PhysicalAddress?.EnsureUniqueDbid();
    }

    internal override void SetNonOwnedAssociations(IRepository repository) {
      base.SetNonOwnedAssociations(repository);

      PhysicalAddress?.SetNonOwnedAssociations(repository);
    }
  }
}


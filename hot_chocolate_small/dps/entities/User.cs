using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.dps.Repositories;

namespace x10.hotchoc.dps.Entities {
  /// <summary>
  /// A portion of Core User entity
  /// </summary>
  public class User : Base {
    // Regular Attributes
    [GraphQLNonNullType]
    public string? Name { get; set; }
    [GraphQLNonNullType]
    public string? Email { get; set; }
    [GraphQLNonNullType]
    public string? Phone { get; set; }
    [GraphQLNonNullType]
    public string? Location { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return Name; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
    }

    internal override void SetNonOwnedAssociations(IRepository repository) {
      base.SetNonOwnedAssociations(repository);
    }
  }
}


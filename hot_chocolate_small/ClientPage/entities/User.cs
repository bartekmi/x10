using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.ClientPage.Repositories;

namespace x10.hotchoc.ClientPage.Entities {
  /// <summary>
  /// A user account within our system
  /// </summary>
  public class User : Base {
    // Regular Attributes
    [GraphQLNonNullType]
    public string? FirstName { get; set; }
    [GraphQLNonNullType]
    public string? LastName { get; set; }
    [GraphQLNonNullType]
    public string? Phone { get; set; }
    [GraphQLNonNullType]
    public string? Email { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return FirstName + " " + LastName; }
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


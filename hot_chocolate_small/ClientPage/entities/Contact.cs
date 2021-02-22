using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.ClientPage.Repositories;

namespace x10.hotchoc.ClientPage.Entities {
  /// <summary>
  /// Basic contact info
  /// </summary>
  public class Contact : Base {
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
      get { return "Contact: " + Dbid; }
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


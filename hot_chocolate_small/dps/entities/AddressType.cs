using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.dps.Repositories;

namespace x10.hotchoc.dps.Entities {
  /// <summary>
  /// An Address of a Company Entity
  /// </summary>
  public class AddressType : Base {
    // Regular Attributes
    [GraphQLNonNullType]
    public string? Address { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return "AddressType: " + Dbid; }
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


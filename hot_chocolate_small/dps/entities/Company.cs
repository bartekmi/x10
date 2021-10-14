using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.dps.Repositories;

namespace x10.hotchoc.dps.Entities {
  /// <summary>
  /// Corresponds to Core Company
  /// </summary>
  public class Company : Base {
    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return "Company: " + Dbid; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    // Associations
    public Client? Client { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
      Client?.EnsureUniqueDbid();
    }

    internal override void SetNonOwnedAssociations(IRepository repository) {
      base.SetNonOwnedAssociations(repository);

      Client?.SetNonOwnedAssociations(repository);
    }
  }
}


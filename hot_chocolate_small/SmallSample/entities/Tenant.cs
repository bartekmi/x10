using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.SmallSample.Repositories;

namespace x10.hotchoc.SmallSample.Entities {
  /// <summary>
  /// Tenant of a Unit
  /// </summary>
  public class Tenant : Base {
    // Regular Attributes
    [GraphQLNonNullType]
    public string? Name { get; set; }
    [GraphQLNonNullType]
    public string? Phone { get; set; }
    [GraphQLNonNullType]
    public string? Email { get; set; }

    // Associations
    [GraphQLNonNullType]
    public Address? PermanentMailingAddress { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
      PermanentMailingAddress?.EnsureUniqueDbid();
    }

    internal override void SetNonOwnedAssociations(IRepository repository) {
      base.SetNonOwnedAssociations(repository);

      PermanentMailingAddress?.SetNonOwnedAssociations(repository);
    }
  }
}


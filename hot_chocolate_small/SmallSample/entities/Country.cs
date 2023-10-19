using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.SmallSample.Repositories;

namespace x10.hotchoc.SmallSample.Entities {
  /// <summary>
  /// Represents a Country
  /// </summary>
  public class Country : Base {
    // Regular Attributes
    [GraphQLNonNullType]
    public string? Code { get; set; }
    [GraphQLNonNullType]
    public string? Name { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
    }

    internal override void SetNonOwnedAssociations(IRepository repository) {
      base.SetNonOwnedAssociations(repository);
    }
  }
}


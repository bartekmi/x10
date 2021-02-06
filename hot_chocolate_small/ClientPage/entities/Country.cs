using System;
using System.Collections.Generic;

using HotChocolate;

namespace x10.hotchoc.ClientPage.Entities {
  /// <summary>
  /// Represents a Country
  /// </summary>
  public class Country : Base {
    // Regular Attributes
    [GraphQLNonNullType]
    public string? Code { get; set; }
    [GraphQLNonNullType]
    public string? Name { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return Name; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    // Associations
    [GraphQLNonNullType]
    public List<StateOrProvince>? SubRegions { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
      SubRegions?.ForEach(x => x.EnsureUniqueDbid());
    }
  }
}


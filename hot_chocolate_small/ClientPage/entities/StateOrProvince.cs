using System;
using System.Collections.Generic;

using HotChocolate;

namespace x10.hotchoc.ClientPage.Entities {
  /// <summary>
  /// State, province or similar division within a country
  /// </summary>
  public class StateOrProvince : Base {
    // Regular Attributes
    [GraphQLNonNullType]
    public string? Name { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return Name; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
    }
  }
}


using System;
using System.Collections.Generic;

using HotChocolate;

namespace x10.hotchoc.ClientPage.Entities {
  /// <summary>
  /// VAT Number
  /// </summary>
  public class VatNumber : Base {
    // Regular Attributes
    [GraphQLNonNullType]
    public string? Number { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return "VatNumber: " + Dbid; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    // Associations
    [GraphQLNonNullType]
    public Country? CountryRegion { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
    }
  }
}


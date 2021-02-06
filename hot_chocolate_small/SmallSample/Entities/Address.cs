using System;
using System.Collections.Generic;

using HotChocolate;

namespace x10.hotchoc.SmallSample.Entities {
  /// <summary>
  /// A physical or mailing address
  /// </summary>
  public class Address : Base {
    // Regular Attributes
    [GraphQLNonNullType]
    public string? UnitNumber { get; set; }
    [GraphQLNonNullType]
    public string? TheAddress { get; set; }
    [GraphQLNonNullType]
    public string? City { get; set; }
    [GraphQLNonNullType]
    public string? StateOrProvince { get; set; }
    [GraphQLNonNullType]
    public string? Zip { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return "Address: " + Dbid; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
    }
  }
}


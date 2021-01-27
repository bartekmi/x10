using System;
using System.Collections.Generic;

using HotChocolate;

namespace x10.hotchoc.Entities {
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
    public string? ToStringRepresentation => "Address: " + Dbid;

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
    }
  }
}


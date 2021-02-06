using System;
using System.Collections.Generic;

using HotChocolate;

namespace x10.hotchoc.ClientPage.Entities {
  /// <summary>
  /// A physical or mailing address
  /// </summary>
  public class Address : Base {
    // Regular Attributes
    [GraphQLNonNullType]
    public string? TheAddress { get; set; }
    [GraphQLNonNullType]
    public string? TheAddress2 { get; set; }
    [GraphQLNonNullType]
    public string? City { get; set; }
    [GraphQLNonNullType]
    public string? PostalCode { get; set; }
    [GraphQLNonNullType]
    public bool Verified { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return "Address: " + Dbid; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    // Associations
    [GraphQLNonNullType]
    public Country? Country { get; set; }
    [GraphQLNonNullType]
    public StateOrProvince? StateOrProvince { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
    }
  }
}


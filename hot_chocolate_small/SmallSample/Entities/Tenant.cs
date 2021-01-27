using System;
using System.Collections.Generic;

using HotChocolate;

namespace x10.hotchoc.Entities {
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

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation => "Tenant: " + Dbid;

    // Associations
    [GraphQLNonNullType]
    public Address? PermanentMailingAddress { get; set; }

  }
}


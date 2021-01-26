using System;
using System.Collections.Generic;
using HotChocolate;

namespace x10.hotchoc.Entities {
  public class Tenant : EntityBase {

    // Regular Attributes
    [GraphQLNonNullType]
    public string? Name { get; set; }
    [GraphQLNonNullType]
    public string? Phone { get; set; }
    [GraphQLNonNullType]
    public string? Email { get; set; }

    // Derived
    [GraphQLNonNullType]
    public string? ToStringRepresentation => Name;

    // Associations
    [GraphQLNonNullType]
    public Address? PermanentMailingAddress { get; set; }
  }
}

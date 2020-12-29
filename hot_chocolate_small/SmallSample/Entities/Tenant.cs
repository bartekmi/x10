using System;
using System.Collections.Generic;
using HotChocolate;

namespace Small.Entities {
  public class Tenant : EntityBase {

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
  }
}

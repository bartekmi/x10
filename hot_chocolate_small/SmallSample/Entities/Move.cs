using System;
using System.Collections.Generic;

using HotChocolate;

namespace x10.hotchoc.Entities {
  /// <summary>
  /// Somewhat contrived move event from one apartment to another
  /// </summary>
  public class Move : Base {
    // Regular Attributes
    [GraphQLNonNullType]
    public DateTime? Date { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation => "Move: " + Dbid;

    // Associations
    [GraphQLNonNullType]
    public Building? From { get; set; }
    [GraphQLNonNullType]
    public Building? To { get; set; }
    [GraphQLNonNullType]
    public Tenant? Tenant { get; set; }

  }
}


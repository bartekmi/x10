using System;
using System.Collections.Generic;

using HotChocolate;

namespace x10.hotchoc.SmallSample.Entities {
  // Enums
  public enum NumberOfBathroomsEnum {
    Half,
    One,
    OneAndHalf,
    Two,
    Three,
    FourPlus,
  }


  /// <summary>
  /// An individual rental unit
  /// </summary>
  public class Unit : Base {
    // Regular Attributes
    [GraphQLNonNullType]
    public string? Number { get; set; }
    public double? SquareFeet { get; set; }
    [GraphQLNonNullType]
    public int? NumberOfBedrooms { get; set; }
    [GraphQLNonNullType]
    public NumberOfBathroomsEnum? NumberOfBathrooms { get; set; }
    [GraphQLNonNullType]
    public bool HasBalcony { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return Number; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
    }
  }
}


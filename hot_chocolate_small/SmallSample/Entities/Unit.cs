using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.SmallSample.Repositories;

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
    public int? NumberOfBedrooms { get; set; }
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

    internal override void SetNonOwnedAssociations(IRepository repository) {
      base.SetNonOwnedAssociations(repository);
    }
  }
}


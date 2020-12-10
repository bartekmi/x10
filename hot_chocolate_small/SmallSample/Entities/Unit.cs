using System;
using System.Collections.Generic;

using HotChocolate;

namespace Small.Entities {

  public enum NumberOfBathroomsEnum {
    Half,
    One,
    OneAndHalf,
    Two,
    Three,
    FourPlus,
  }

  public class Unit : EntityBase {
    [GraphQLNonNullType]
    public string? Number { get; set;}
    public double? SquareFeet { get; set;}
    [GraphQLNonNullType]
    public int? NumberOfBedrooms { get; set;}
    [GraphQLNonNullType]
    public NumberOfBathroomsEnum? NumberOfBathrooms { get; set;}
    [GraphQLNonNullType]
    public bool HasBalcony { get; set;}
  }
}

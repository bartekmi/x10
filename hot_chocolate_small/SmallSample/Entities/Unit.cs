using System;
using System.Collections.Generic;
using Newtonsoft.Json;

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
    public string? Number { get; set;}
    public double? SquareFeet { get; set;}
    public int? NumberOfBedrooms { get; set;}
    public NumberOfBathroomsEnum? NumberOfBathrooms { get; set;}
    public bool HasBalcony { get; set;}
  }
}

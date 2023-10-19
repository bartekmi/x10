using System;
using System.Collections.Generic;

using wpf_lib.lib;
using wpf_lib.lib.attributes;

using wpf_generated.functions;

namespace wpf_generated.entities {

  public enum NumberOfBathroomsEnum {
    [Label("Half")]
    Half,
    [Label("1")]
    One,
    [Label("1.5")]
    OneAndHalf,
    [Label("2")]
    Two,
    [Label("3")]
    Three,
    [Label("4+")]
    FourPlus,
  }


  public class Unit : EntityBase {

    // Regular Attributes
    private string _number;
    public string Number {
      get { return _number; }
      set {
        _number = value;
        RaisePropertyChanged(nameof(Number));
      }
    }
    private double? _squareFeet;
    public double? SquareFeet {
      get { return _squareFeet; }
      set {
        _squareFeet = value;
        RaisePropertyChanged(nameof(SquareFeet));
      }
    }
    private int? _numberOfBedrooms;
    public int? NumberOfBedrooms {
      get { return _numberOfBedrooms; }
      set {
        _numberOfBedrooms = value;
        RaisePropertyChanged(nameof(NumberOfBedrooms));
      }
    }
    private NumberOfBathroomsEnum? _numberOfBathrooms;
    public NumberOfBathroomsEnum? NumberOfBathrooms {
      get { return _numberOfBathrooms; }
      set {
        _numberOfBathrooms = value;
        RaisePropertyChanged(nameof(NumberOfBathrooms));
      }
    }
    private bool _hasBalcony;
    public bool HasBalcony {
      get { return _hasBalcony; }
      set {
        _hasBalcony = value;
        RaisePropertyChanged(nameof(HasBalcony));
      }
    }

    // Derived Attributes

    // Associations

    public override string ToString() {
      return Number?.ToString();
    }

    public static Unit Create() {
      return new Unit {
        NumberOfBedrooms = 2,
        NumberOfBathrooms = NumberOfBathroomsEnum.One,
        HasBalcony = false,
      };
    }
  }
}

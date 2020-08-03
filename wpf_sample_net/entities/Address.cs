using System;
using System.Collections.Generic;

using wpf_lib.lib;
using wpf_lib.lib.attributes;

using wpf_generated.functions;

namespace wpf_generated.entities {


  public class Address : EntityBase {

    // Regular Attributes
    private string _unitNumber;
    public string UnitNumber {
      get { return _unitNumber; }
      set {
        _unitNumber = value;
        RaisePropertyChanged(nameof(UnitNumber));

        RaisePropertyChanged(nameof(FirstAddressLine));
      }
    }
    private string _theAddress;
    public string TheAddress {
      get { return _theAddress; }
      set {
        _theAddress = value;
        RaisePropertyChanged(nameof(TheAddress));

        RaisePropertyChanged(nameof(FirstAddressLine));
      }
    }
    private string _city;
    public string City {
      get { return _city; }
      set {
        _city = value;
        RaisePropertyChanged(nameof(City));

        RaisePropertyChanged(nameof(SecondAddressLine));
      }
    }
    private string _stateOrProvince;
    public string StateOrProvince {
      get { return _stateOrProvince; }
      set {
        _stateOrProvince = value;
        RaisePropertyChanged(nameof(StateOrProvince));

        RaisePropertyChanged(nameof(SecondAddressLine));
      }
    }
    private string _zip;
    public string Zip {
      get { return _zip; }
      set {
        _zip = value;
        RaisePropertyChanged(nameof(Zip));

        RaisePropertyChanged(nameof(ThirdAddressLine));
      }
    }

    // Derived Attributes
    public string FirstAddressLine {
      get {
        return TheAddress + "   Unit " + UnitNumber;
      }
    }
    public string SecondAddressLine {
      get {
        return City + ", " + StateOrProvince;
      }
    }
    public string ThirdAddressLine {
      get {
        return Zip;
      }
    }

    // Associations

    // Validations
    public override void CalculateErrors(string prefix, EntityErrors errors) {
      if (string.IsNullOrWhiteSpace(TheAddress?.ToString()))
        errors.Add("Name is required", prefix, nameof(TheAddress));
    }

    public static Address Create(EntityBase owner) {
      return new Address {
        Owner = owner,
      };
    }
  }
}

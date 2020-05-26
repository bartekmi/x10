using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Remoting.Contexts;
using wpf_sample.lib;

namespace wpf_sample.entities.booking {
  public class Port : EntityBase {

    // Regular Attributes
    private string _name;
    [Column("name")]
    public string Name {
      get { return _name; }
      set {
        if (value != _name) {
          _name = value;
          RaisePropertyChanged(nameof(Name));
        }
      }
    }

    private string _city;
    [Column("city")]
    public string City {
      get { return _city; }
      set {
        if (value != _city) {
          _city = value;
          RaisePropertyChanged(nameof(City));
        }
      }
    }

    private string _stateOrProvince;
    [Column("state_or_province")]
    public string StateOrProvince {
      get { return _stateOrProvince; }
      set {
        if (value != _stateOrProvince) {
          _stateOrProvince = value;
          RaisePropertyChanged(nameof(StateOrProvince));
        }
      }
    }

    private string _countryCode;
    [Column("country_code")]
    public string CountryCode {
      get { return _countryCode; }
      set {
        if (value != _countryCode) {
          _countryCode = value;
          RaisePropertyChanged(nameof(CountryCode));
        }
      }
    }



    public override string ToString() {
      return City;
    }
  }
}

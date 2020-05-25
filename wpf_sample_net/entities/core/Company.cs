using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using wpf_sample.lib;

namespace wpf_sample.entities.booking {
  public class Company : EntityBase {
    private string _legalName;
    [Column("legal_name")]
    public string LegalName {
      get { return _legalName; }
      set {
        if (value != _legalName) {
          _legalName = value;
          RaisePropertyChanged(nameof(LegalName));
        }
      }
    }

    private string _address;
    [Column("address")]
    public string Address {
      get { return _address; }
      set {
        if (value != _address) {
          _address = value;
          RaisePropertyChanged(nameof(Address));
        }
      }
    }

    public override string ToString() {
      return LegalName;
    }
  }
}

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
  }
}

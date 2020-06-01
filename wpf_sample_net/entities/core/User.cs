using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using wpf_lib.lib;

namespace wpf_sample.entities.core {
  public class User : EntityBase {

    // Regular Attributes
    private string _firstName;
    [Column("first_name")]
    public string FirstName {
      get { return _firstName; }
      set {
        _firstName = value;
        RaisePropertyChanged(nameof(FirstName));
      }
    }

    private string _lastName;
    [Column("last_name")]
    public string LastName {
      get { return _lastName; }
      set {
        _lastName = value;
        RaisePropertyChanged(nameof(LastName));
      }
    }

    // Associations
    public virtual Company Company { get; set; }


    public override string ToString() {
      return string.Format("{0} {1}", FirstName, LastName);
    }

  }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using wpf_sample.lib;

namespace wpf_sample.entities.booking {
  public class User : EntityBase {

    // Regular Attributes
    private string _firstName;
    [Column("first_name")]
    public string FirstName {
      get { return _firstName; }
      set {
        if (value != _firstName) {
          _firstName = value;
          RaisePropertyChanged(nameof(FirstName));
        }
      }
    }

    // Associations
    public virtual Company Company { get; set; }
  }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wpf_sample.lib {
  public abstract class EntityBase : NotificationObject {

    private int _id;
    [Key]
    [Column("id")]
    public int Id {
      get { return _id; }
      set {
        if (value != _id) {
          _id = value;
          RaisePropertyChanged(nameof(Id));
        }
      }
    }
  }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wpf_lib.lib {
  public abstract class EntityBase : NotificationObject {

    // Entities override this to implement error validation
    public virtual void CalculateErrors(EntityErrors errors) { }

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

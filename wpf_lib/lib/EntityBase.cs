using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wpf_lib.lib {
  public abstract class EntityBase : NotificationObject {

    private const int IS_NEW = int.MinValue;

    // Entities override this to implement error validation
    public virtual void CalculateErrors(string prefix, EntityErrors errors) { }

    private int _id = IS_NEW;
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

    public bool IsNew() {
      return Id == IS_NEW;
    }
  }
}

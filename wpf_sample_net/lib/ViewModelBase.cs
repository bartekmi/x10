using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_sample.lib {
  public abstract class ViewModelBase : NotificationObject {
    internal EntityBase ModelUntyped { get; set; }
  }

  public abstract class ViewModelBase<T> : ViewModelBase where T : EntityBase {
    public T Model {
      get { return (T)ModelUntyped; }
      set {
        ModelUntyped = value;
        RaisePropertyChanged(nameof(Model));
      }
    }
  }
}

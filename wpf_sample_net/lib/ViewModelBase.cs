using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_sample.lib {
  public abstract class ViewModelBase : NotificationObject {
    internal EntityBase ModelUntyped { get; set; }
  }

  public abstract class ViewModelBase<T> : ViewModelBase where T : EntityBase {

    // Individual View Models override this to fire notification changes
    // on properties defined in the 'custom' partial class definition
    public virtual void FireCustomPropertyNotification() { }

    public T Model {
      get { return (T)ModelUntyped; }
      set {
        if (ModelUntyped != null)
          ModelUntyped.PropertyChanged -= ModelUntyped_PropertyChanged;

        ModelUntyped = value;
        if (ModelUntyped != null)
          ModelUntyped.PropertyChanged += ModelUntyped_PropertyChanged;

        RaisePropertyChanged(nameof(Model));
      }
    }

    private void ModelUntyped_PropertyChanged(object sender, PropertyChangedEventArgs e) {
      FireCustomPropertyNotification();
    }
  }
}

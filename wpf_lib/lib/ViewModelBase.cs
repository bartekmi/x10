using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace wpf_lib.lib {
  public abstract class ViewModelBase : NotificationObject {
    internal EntityBase ModelUntyped { get; set; }
  }

  public abstract class ViewModelBase<T> : ViewModelBase where T : EntityBase {

    // View Models override this to fire notification changes
    // on properties defined in the 'custom' partial class definition
    public virtual void FireCustomPropertyNotification() { }

    // View Models override this to implement error validation
    public virtual FormErrors CalculateErrors() { return new FormErrors(); }

    protected bool ShowErrors = true; // TODO: Only show after submit pressed

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

      if (ShowErrors) {
        FormErrors errors = CalculateErrors();
        PopulateErrors(errors);
      }
    }

    private UserControl _userControl;
    protected ViewModelBase(UserControl userControl) {
      _userControl = userControl;
    }

    private void PopulateErrors(FormErrors errors) {
      Form.ShowErrors(_userControl, errors);
    }
  }
}

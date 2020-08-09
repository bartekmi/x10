using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace wpf_lib.lib {
  public abstract class ViewModelBase : NotificationObject {
    internal object ModelUntyped { get; set; }
  }

  public abstract class ViewModelBase<T> : ViewModelBase where T : EntityBase {

    // View Models override this to fire notification changes
    // on properties defined in the 'custom' partial class definition
    public virtual void FireCustomPropertyNotification() { }

    public bool ShowErrors { get; private set; }

    public T Model {
      get { return (T)ModelUntyped; }
      set {
        if (ModelUntyped != null)
          Model.PropertyChanged -= ModelUntyped_PropertyChanged;

        ModelUntyped = value;
        if (ModelUntyped != null)
          Model.PropertyChanged += ModelUntyped_PropertyChanged;

        RaisePropertyChanged(nameof(Model));
      }
    }

    private void ModelUntyped_PropertyChanged(object sender, PropertyChangedEventArgs e) {
      FireCustomPropertyNotification();

      if (ShowErrors)
        CalculateAndPopulateErrors();
    }

    private bool CalculateAndPopulateErrors() {
      EntityErrors errors = new EntityErrors();
      Model.CalculateErrors(null, errors);
      PopulateErrors(errors);
      return errors.HasErrors;
    }

    private UserControl _userControl;
    protected ViewModelBase(UserControl userControl) {
      _userControl = userControl;
    }

    public void SubmitData(Action submitAction, string successMessage) {
      ShowErrors = true;
      bool hasErrors = CalculateAndPopulateErrors();

      if (hasErrors) {
        MessageBox.Show("Please fix your errors, first", "Errors", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        return;
      }

      try {
        submitAction();
        MessageBox.Show(successMessage, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
      } catch (Exception e) {
        MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
      }
    }

    private void PopulateErrors(EntityErrors errors) {
      Form.ShowErrors(_userControl, errors);
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wpf_sample.lib.utils;

namespace wpf_sample.lib {
  public partial class Form : UserControl {

    private List<FormErrorDisplay> _errorDisplays = new List<FormErrorDisplay>();
    private List<EditElementWrapper> _editWrappers = new List<EditElementWrapper>();

    public Form() {
      InitializeComponent();
    }

    internal void ShowErrors(FormErrors errors) {
      foreach (FormErrorDisplay display in _errorDisplays)
        display.DisplayErrors(errors);

      foreach (EditElementWrapper wrapper in _editWrappers) {
        IEnumerable<FormError> errorsForField = errors.ErrorsForField(wrapper.EditorFor);
        wrapper.DisplayErrors(errorsForField);
      }
    }

    #region Helper Methods
    internal static void ShowErrors(UserControl userControl, FormErrors errors) {
      if (userControl.Content is Form form) 
        form.ShowErrors(errors);
    }

    internal static void RegisterErrorDisplay(FormErrorDisplay errorDisplay) {
      Form form = WpfUtils.FindAncestor<Form>(errorDisplay);
      if (form != null)
        form._errorDisplays.Add(errorDisplay);
    }

    internal static void RegisterEditWrapper(EditElementWrapper wrapper) {
      Form form = WpfUtils.FindAncestor<Form>(wrapper);
      if (form != null)
        form._editWrappers.Add(wrapper);
    }
    #endregion
  }
}

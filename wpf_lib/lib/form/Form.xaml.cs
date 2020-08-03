using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

using wpf_lib.lib.utils;

namespace wpf_lib.lib {
  [ContentProperty(nameof(Children))]
  public partial class Form : UserControl {
    public static readonly DependencyPropertyKey ChildrenProperty = DependencyProperty.RegisterReadOnly(
        nameof(Children),
        typeof(UIElementCollection),
        typeof(Form),
        new PropertyMetadata());
    public UIElementCollection Children {
      get { return (UIElementCollection)GetValue(ChildrenProperty.DependencyProperty); }
      private set { SetValue(ChildrenProperty, value); }
    }

    private List<FormErrorDisplay> _errorDisplays = new List<FormErrorDisplay>();
    private List<EditElementWrapper> _editWrappers = new List<EditElementWrapper>();

    public Form() {
      InitializeComponent();
      Children = PART_Host.Children;
    }

    internal void ShowErrors(EntityErrors errors) {
      foreach (FormErrorDisplay display in _errorDisplays)
        display.DisplayErrors(errors);

      foreach (EditElementWrapper wrapper in _editWrappers) {
        IEnumerable<EntityError> errorsForField = errors.ErrorsForField(wrapper.EditorFor);
        wrapper.DisplayErrors(errorsForField);
      }
    }

    #region Helper Methods
    internal static void ShowErrors(UserControl userControl, EntityErrors errors) {
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

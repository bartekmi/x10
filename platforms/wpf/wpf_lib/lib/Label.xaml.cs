using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace wpf_lib.lib {
  public partial class Label : UserControl {
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text),
        typeof(string),
        typeof(Label)
    );
    public string Text {
      get { return (string)GetValue(TextProperty); }
      set { SetValue(TextProperty, value); }
    }

    public static readonly DependencyProperty IsMandatoryProperty = DependencyProperty.Register(
        nameof(IsMandatory),
        typeof(bool),
        typeof(Label)
      );
    public bool IsMandatory {
      get { return (bool)GetValue(IsMandatoryProperty); }
      set { SetValue(IsMandatoryProperty, value); }
    }

    public static readonly DependencyProperty MyToolTipProperty = DependencyProperty.Register(
        nameof(MyToolTip),
        typeof(string),
        typeof(Label)
      );
    public string MyToolTip {
      get { return (string)GetValue(MyToolTipProperty); }
      set { SetValue(MyToolTipProperty, value); }
    }

    public Label() {
      InitializeComponent();
    }
  }
}

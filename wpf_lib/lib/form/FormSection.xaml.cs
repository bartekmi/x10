using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wpf_lib.lib {
  [ContentProperty(nameof(Children))]
  public partial class FormSection : UserControl {
    public static readonly DependencyPropertyKey ChildrenProperty = DependencyProperty.RegisterReadOnly(
        nameof(Children),
        typeof(UIElementCollection),
        typeof(FormSection),
        new PropertyMetadata());
    public UIElementCollection Children {
      get { return (UIElementCollection)GetValue(ChildrenProperty.DependencyProperty); }
      private set { SetValue(ChildrenProperty, value); }
    }

    public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(
        nameof(Label),
        typeof(string),
        typeof(FormSection),
        new FrameworkPropertyMetadata() {
          PropertyChangedCallback = new PropertyChangedCallback((s, e) => ((FormSection)s).PART_Header.Text = (string)e.NewValue),
        });
    public string Label {
      get { return (string)GetValue(LabelProperty); }
      set { SetValue(LabelProperty, value); }
    }

    public FormSection() {
      InitializeComponent();
      Children = PART_Host.Children;
    }
  }
}

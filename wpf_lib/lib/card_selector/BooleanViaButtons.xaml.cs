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

namespace wpf_sample.lib {
  public partial class BooleanViaButtons : UserControl {
    public static readonly DependencyProperty SelectedProperty = DependencyProperty.Register(
        nameof(Selected),
        typeof(object),
        typeof(BooleanViaButtons),
        new FrameworkPropertyMetadata() {
          BindsTwoWayByDefault = true,
          DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
        }
      );
    public object Selected {
      get { return GetValue(SelectedProperty); }
      set { SetValue(SelectedProperty, value); }
    }

    public static readonly DependencyProperty TextForTrueProperty = DependencyProperty.Register(
        nameof(TextForTrue),
        typeof(string),
        typeof(BooleanViaButtons)
      );
    public string TextForTrue {
      get { return (string)GetValue(TextForTrueProperty); }
      set { SetValue(TextForTrueProperty, value); }
    }

    public static readonly DependencyProperty TextForFalseProperty = DependencyProperty.Register(
        nameof(TextForFalse),
        typeof(string),
        typeof(BooleanViaButtons)
      );
    public string TextForFalse {
      get { return (string)GetValue(TextForFalseProperty); }
      set { SetValue(TextForFalseProperty, value); }
    }

    public BooleanViaButtons() {
      InitializeComponent();

      Loaded += (s, e) => {
        uxCardSelector.SetCardInfos(new List<CardInfo>() {
          new CardInfo() {
            Value = true,
            Label = TextForTrue,
          },
          new CardInfo() {
            Value = false,
            Label = TextForFalse,
          },
        });
      };
    }
  }
}

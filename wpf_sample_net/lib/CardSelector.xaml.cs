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

namespace wpf_sample.lib {
  public partial class CardSelector : UserControl {
    public static readonly DependencyProperty SelectedProperty = DependencyProperty.Register(
        nameof(Selected),
        typeof(string),
        typeof(CardSelector)
      );
    public string Selected {
      get { return (string)GetValue(SelectedProperty); }
      set { SetValue(SelectedProperty, value); }
    }

    public static readonly DependencyProperty ItemsSourceEnumProperty = DependencyProperty.Register(
        nameof(ItemsSourceEnum),
        typeof(string),
        typeof(CardSelector),
        new PropertyMetadata(
          new PropertyChangedCallback((o, ea) => {
              Console.WriteLine(ea.NewValue);
            }
          )
        )
      );
    public string ItemsSourceEnum {
      get { return (string)GetValue(ItemsSourceEnumProperty); }
      set { SetValue(ItemsSourceEnumProperty, value); }
    }

    public CardSelector() {
      InitializeComponent();
    }
  }
}

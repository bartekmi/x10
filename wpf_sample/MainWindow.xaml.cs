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

namespace wpf_sample {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();

      uxControlSelector.ItemsSource = Components()
        .Select(x => new ControlTypeWrapper(x))
        .OrderBy(x => x.ToString());

      uxControlSelector.SelectionChanged += UxControlSelector_SelectionChanged;
    }

    private void UxControlSelector_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      ControlTypeWrapper wrapper = (ControlTypeWrapper)uxControlSelector.SelectedItem;
      uxContent.Children.Clear();
      uxContent.Children.Add(wrapper.CreateUserControl());
    }

    private static List<Type> Components() {
      return new List<Type>() {
        typeof(BookingForm),
        typeof(Bookings),
      };
    }

    class ControlTypeWrapper {
      private Type _type;

      internal ControlTypeWrapper(Type type) {
        _type = type;
      }

      internal UserControl CreateUserControl() {
        return (UserControl)Activator.CreateInstance(_type);
      }

      public override string ToString() {
        return _type.Name;
      }
    }
  }
}

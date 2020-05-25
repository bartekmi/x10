using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using wpf_sample.entities;
using wpf_sample.ui.bookings;

using wpf_sample.entities.booking;
using Newtonsoft.Json;
using wpf_sample.lib;

namespace wpf_sample {
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();
      InitializeContext();

      uxControlSelector.ItemsSource = Components()
        .Select(x => new ControlTypeWrapper(x))
        .OrderBy(x => x.ToString());

      uxControlSelector.SelectionChanged += UxControlSelector_SelectionChanged;
    }

    private void InitializeContext() {
      AppStatics.Singleton.Context = new __Context__() {
        User = new User() {
          FirstName = "Bartek",
          LastName = "Muszynski",
          Company = AppStatics.Singleton.DataSource.Companies.First(),
        }
      };
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
      private readonly Type _type;

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

    private void PrintModel(object sender, RoutedEventArgs e) {
      FrameworkElement element = (FrameworkElement)uxContent.Children[0];
      EntityBase entity = ((ViewModelBase)element.DataContext).ModelUntyped;
      string json = JsonConvert.SerializeObject(entity, Formatting.Indented);
      Console.WriteLine(json);
    }
  }
}

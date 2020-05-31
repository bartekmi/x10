using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Newtonsoft.Json;
using wpf_sample.lib;

namespace wpf_lib {
  public partial class WpfStoryBook : UserControl {
    public WpfStoryBook() {
      InitializeComponent();
      uxControlSelector.SelectionChanged += UxControlSelector_SelectionChanged;
    }

    public void InitializeComponents(IEnumerable<Type> types) {
      uxControlSelector.ItemsSource = types
        .Select(x => new ControlTypeWrapper(x))
        .OrderBy(x => x.ToString());
    }

    private void UxControlSelector_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      ControlTypeWrapper wrapper = (ControlTypeWrapper)uxControlSelector.SelectedItem;
      uxContent.Children.Clear();
      uxContent.Children.Add(wrapper.CreateUserControl());
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

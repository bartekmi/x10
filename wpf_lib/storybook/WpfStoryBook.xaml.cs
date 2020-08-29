using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Newtonsoft.Json;
using wpf_lib.lib;

namespace wpf_lib.storybook {
  public partial class WpfStoryBook : UserControl, INavigation {

    private IEnumerable<ControlTypeWrapper> _wrappers;

    public WpfStoryBook() {
      InitializeComponent();
      uxTextBoxUrl.KeyDown += UxTextBoxUrl_KeyDown;
    }

    public void InitializeComponents(IEnumerable<Type> types) {
      _wrappers = types.Select(x => new ControlTypeWrapper(x));
    }

    public void NavigateToUrl(string url) {
      uxTextBoxUrl.Text = url;

      foreach (ControlTypeWrapper wrapper in _wrappers) {
        if (wrapper.CorrespondsToUrl(url, out Parameters parameters)) {
          TopLevelControlBase control = wrapper.GetUserControl();
          control.ViewModelBase.PopulateData(parameters);
          uxContent.Children.Clear();
          uxContent.Children.Add(control);
          return;
        }
      }

      MessageBox.Show("URL not recognized: " + url, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void UxTextBoxUrl_KeyDown(object sender, KeyEventArgs e) {
      if (e.Key == Key.Return) {
        string url = uxTextBoxUrl.Text;
        NavigateToUrl(url);
      }
    }

    private void PrintModel(object sender, RoutedEventArgs e) {
      FrameworkElement element = (FrameworkElement)uxContent.Children[0];
      object model = ((ViewModelBase)element.DataContext).ModelUntyped;
      string json = JsonConvert.SerializeObject(model, Formatting.Indented);
      Console.WriteLine(json);
    }
  }
}

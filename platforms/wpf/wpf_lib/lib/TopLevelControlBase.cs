using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using wpf_lib.lib.url_parsing;
using wpf_lib.storybook;

namespace wpf_lib.lib {
  public abstract class TopLevelControlBase : UserControl {
    public ViewModelBase ViewModelBase { get { return (ViewModelBase)DataContext; } }

    public string Url { get; set; }
    public string Query { get; set; }

    public void NavigateToUrlInTag(object sender, RoutedEventArgs e) {
      FrameworkElement element = (FrameworkElement)sender;
      EntityBase model = (EntityBase)element.DataContext;
      string url = element.Tag.ToString();
      url = ParsedUrl.Substitute(url, model);
      BaseAppStatics.BaseSingleton.Navigation.NavigateToUrl(url);
    }
  }
}

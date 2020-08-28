using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using wpf_lib.storybook;

namespace wpf_lib.lib {
  public abstract class TopLevelControlBase : UserControl {
    public ViewModelBase ViewModelBase { get { return (ViewModelBase)DataContext; } }

    public string Url { get; set; }
    public string Query { get; set; }
  }
}

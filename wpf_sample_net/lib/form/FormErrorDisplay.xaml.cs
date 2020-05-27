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
  public partial class FormErrorDisplay : UserControl {
    public FormErrorDisplay() {
      InitializeComponent();
      Loaded += (s, e) => Form.RegisterErrorDisplay(this);
    }

    internal void DisplayErrors(FormErrors errors) {
      Visibility = errors.HasErrors ? Visibility.Visible : Visibility.Collapsed;
      uxContent.Text = string.Join("\n\r", errors.Errors.Select(x => x.Message));
    }
  }
}

using System.Windows;
using System.Windows.Controls;

namespace wpf_generated.ui {
  public partial class Buildings : UserControl {

    private BuildingsVM ViewModel { get { return (BuildingsVM)DataContext; } }

    public Buildings() {
      InitializeComponent();
      DataContext = new BuildingsVM(this);
    }
  }
}

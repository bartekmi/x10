using System.Windows;
using System.Windows.Controls;
using wpf_lib.lib;

namespace wpf_generated.ui {
  public partial class Buildings : TopLevelControlBase {

    private BuildingsVM ViewModel { get { return (BuildingsVM)DataContext; } }

    public Buildings() {
      InitializeComponent();
      DataContext = new BuildingsVM(this);
      Url = "/buildings";
      Query = "Building";
    }
  }
}

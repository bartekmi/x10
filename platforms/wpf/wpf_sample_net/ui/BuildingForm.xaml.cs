using System.Windows;
using System.Windows.Controls;
using wpf_lib.lib;

using wpf_sample;

namespace wpf_generated.ui {
  public partial class BuildingForm : TopLevelControlBase {

    private BuildingFormVM ViewModel { get { return (BuildingFormVM)DataContext; } }

    public BuildingForm() {
      InitializeComponent();
      DataContext = new BuildingFormVM(this);
      Url = "/building/{$buildingId}";
      Query = "GetBuilding(buildingId: $buildingId)";
    }

    // Submit Method(s)
    private void SaveClick(object sender, RoutedEventArgs e) {
      ViewModel.SubmitData(() => AppStatics.Singleton.DataSource.CreateOrUpdate(ViewModel.Model),
        "Saved");
    }
  }
}

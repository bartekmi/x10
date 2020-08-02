using System.Windows;
using System.Windows.Controls;

using wpf_sample;

namespace wpf_generated.ui {
  public partial class BuildingForm : UserControl {

    private BuildingFormVM ViewModel { get { return (BuildingFormVM)DataContext; } }

    public BuildingForm() {
      InitializeComponent();
      DataContext = new BuildingFormVM(this);
    }

    // Submit Method(s)
    private void SaveClick(object sender, RoutedEventArgs e) {
      ViewModel.SubmitData(() => AppStatics.Singleton.DataSource.Create(ViewModel.Model),
        "Saved");
    }

  }
}

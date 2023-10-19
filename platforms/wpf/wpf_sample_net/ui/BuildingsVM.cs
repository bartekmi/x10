using System.Collections.Generic;
using System.Windows.Controls;

using wpf_lib.lib;
using wpf_lib.lib.utils;

using wpf_generated.entities;
using wpf_sample;

namespace wpf_generated.ui {
  public partial class BuildingsVM : ViewModelBaseMany<Building> {
    public BuildingsVM(UserControl userControl) : base(userControl) {
      // Do nothing
    }

    public override void PopulateData(Parameters parameters) {
      Model = AppStatics.Singleton.DataSource.Buildings;
    }
  }
}

using System.Collections.Generic;
using System.Windows.Controls;

using wpf_lib.lib;
using wpf_lib.lib.utils;

using wpf_generated.entities;
using wpf_sample;
using System.Linq;

namespace wpf_generated.ui {
  public partial class BuildingFormVM : ViewModelBase<Building> {
    // Data Sources
    public IEnumerable<EnumValueRepresentation> MailboxTypes { get; }
      = EnumValueRepresentation.GetForEnumType(typeof(MailboxTypeEnum));
    public IEnumerable<EnumValueRepresentation> PetPolicies { get; }
      = EnumValueRepresentation.GetForEnumType(typeof(PetPolicyEnum));

    // Properties used in XAML bindings
    public bool VerticalStackPanel_Visibility {
      get {
        return !Model.MailingAddressSameAsPhysical;
      }
    }

    // Ensures all properties above are refreshed every time anything changes
    public override void FireCustomPropertyNotification() {
      RaisePropertyChanged(nameof(VerticalStackPanel_Visibility));
    }

    public BuildingFormVM(UserControl userControl) : base(userControl) {
      // Do nothing
    }

    public override void PopulateData(Parameters parameters) {
      string id = parameters.Single();
      if (id == NEW_ENTITY_URL_TAG)
        Model = Building.Create(null);
      else
        Model = AppStatics.Singleton.DataSource.GetById<Building>(int.Parse(id));
    }
  }
}

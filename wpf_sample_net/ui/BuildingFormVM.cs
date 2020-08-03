using System.Collections.Generic;
using System.Windows.Controls;

using wpf_lib.lib;
using wpf_lib.lib.utils;

using wpf_generated.entities;

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
      Model = Building.Create(null);
    }
  }
}

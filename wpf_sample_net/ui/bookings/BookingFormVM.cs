using System.Collections.Generic;
using System.Windows.Controls;

using wpf_lib.lib;

using wpf_sample.entities.booking;
using wpf_sample.entities.core;

namespace wpf_sample.ui.bookings {
  public partial class BookingFormVM : ViewModelBase<Booking> {
    // State
    private bool _notifyPartySameAsConsignee;
    public bool NotifyPartySameAsConsignee {
      get { return _notifyPartySameAsConsignee; }
      set {
        if (value != _notifyPartySameAsConsignee) {
          _notifyPartySameAsConsignee = value;
          RaisePropertyChanged(nameof(NotifyPartySameAsConsignee));
        }
      }
    }

    // Conditions from YAML
    public bool IsLclVisibility {
      get { return Model.TransportationMode == TransportationMode.Ocean; }
    }
    public bool IsLtlVisibility {
      get { return Model.TransportationMode == TransportationMode.Truck; }
    }
    public bool OriginPortVisibility {
      get { return Model.TransportationMode != TransportationMode.Truck; }
    }

    public override void FireCustomPropertyNotification() {
      RaisePropertyChanged(nameof(IsLclVisibility));
      RaisePropertyChanged(nameof(IsLtlVisibility));
      RaisePropertyChanged(nameof(OriginPortVisibility));
    }

    // Validations
    public override FormErrors CalculateErrors() {
      FormErrors errors = new FormErrors();

      if (Model.TargetDeliveryDate < Model.CargoReadyDate)
        errors.Add("Target Delivery Date can't be before Cargo Ready Date", 
          nameof(Model.CargoReadyDate), nameof(Model.TargetDeliveryDate));
      if (!Model.WantsOriginService && Model.OriginPort == null)
        errors.Add("Origin Port must be provided when 'Wants Origin Service' is false", nameof(Model.OriginPort));

      return errors;
    }

    // TODO - does this need to be moved to hand-coded custom file?
    public IEnumerable<Company> ShipperCompanies {
      get { return AppStatics.Singleton.DataSource.Companies; }
    }
    public IEnumerable<Company> ConsigneeCompanies {
      get { return AppStatics.Singleton.DataSource.Companies; }
    }
    public IEnumerable<Location> OriginLocations {
      get { return AppStatics.Singleton.DataSource.Locations; }
    }
    public IEnumerable<Port> OriginPorts {
      get { return AppStatics.Singleton.DataSource.Ports; }
    }

    public BookingFormVM(UserControl userControl) : base(userControl) {
      Model = Booking.Create();
    }
  }
}

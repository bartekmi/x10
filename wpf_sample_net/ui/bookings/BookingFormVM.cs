using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using wpf_sample.entities;
using wpf_sample.entities.booking;
using wpf_sample.lib;

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

    public BookingFormVM() {
      Model = new Booking() {
        Shipper = AppStatics.Singleton.Context.User.Company,
      };
    }
  }
}

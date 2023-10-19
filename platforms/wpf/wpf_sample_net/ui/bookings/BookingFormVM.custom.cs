using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using wpf_lib.lib;

using wpf_sample.entities.core;
using wpf_sample.entities.booking;

namespace wpf_sample.ui.bookings {
  public partial class BookingFormVM : ViewModelBase<Booking> {
    internal void SelectConsigneeBooking() {
      Company myCompany = AppStatics.Singleton.Context.User.Company;
      if (myCompany.Id == Model.Consignee?.Id)
        return;
      SwitchShipperAndConsignee();
    }

    internal void SelectShipperBooking() {
      Company myCompany = AppStatics.Singleton.Context.User.Company;
      if (myCompany.Id == Model.Shipper?.Id)
        return;
      SwitchShipperAndConsignee();
    }

    private void SwitchShipperAndConsignee() {
      Company temp = Model.Shipper;
      Model.ShipperBindable = Model.Consignee;
      Model.ConsigneeBindable = temp;
    }
  }
}

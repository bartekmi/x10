using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    public BookingFormVM() {
      Model = new Booking() {
        Shipper = __Context__.Singleton.User.Company,
      };
    }
  }
}

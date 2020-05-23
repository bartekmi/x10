using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Remoting.Contexts;
using wpf_sample.lib;

namespace wpf_sample.entities.booking {
  public class Booking : EntityBase {

    // Regular Attributes
    private string _name;
    [Column("name")]
    public string Name {
      get { return _name; }
      set {
        if (value != _name) {
          _name = value;
          RaisePropertyChanged(nameof(Name));
        }
      }
    }

    private TransportationMode _transportationMode;
    [Column("transportation_mode")]
    public TransportationMode TransportationMode {
      get { return _transportationMode; }
      set {
        if (value != _transportationMode) {
          _transportationMode = value;
          RaisePropertyChanged(nameof(TransportationMode));
        }
      }
    }

    // Derived Attributes
    public bool IsShipperBooking {
      get {
        return Shipper?.Id == __Context__.Singleton.User.Company.Id;
      }
    }

    // Associations
    public virtual Company Shipper { get; set; }
    public virtual Company Consignee { get; set; }
  }
}

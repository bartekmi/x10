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
        _transportationMode = value;
        RaisePropertyChanged(nameof(TransportationMode));
      }
    }

    private string _notifyParty;
    [Column("notify_party")]
    public string NotifyParty {
      get { return _notifyParty; }
      set {
        _notifyParty = value;
        RaisePropertyChanged(nameof(NotifyParty));
      }
    }

    // Derived Attributes
    public bool IsShipperBooking {
      get {
        return Shipper?.Id == __Context__.Singleton.User.Company.Id;
      }
    }
    public bool IsConsigneeBooking {
      get {
        return Consignee?.Id == __Context__.Singleton.User.Company.Id;
      }
    }

    // Associations
    public virtual Company Shipper { get; set; }
    [NotMapped]
    public Company ShipperBindable {
      get { return Shipper; }
      set {
        Shipper = value;
        RaisePropertyChanged(nameof(ShipperBindable));
        RaisePropertyChanged(nameof(IsShipperBooking));
      }
    }

    public virtual Company Consignee { get; set; }
    [NotMapped]
    public Company ConsigneeBindable {
      get { return Consignee; }
      set {
        Consignee = value;
        RaisePropertyChanged(nameof(ConsigneeBindable));
        RaisePropertyChanged(nameof(IsConsigneeBooking));
      }
    }

    public override string ToString() {
      return Name;
    }
  }
}

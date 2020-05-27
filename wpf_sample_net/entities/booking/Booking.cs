using System;
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

    private TransportationMode? _transportationMode;
    [Column("transportation_mode")]
    public TransportationMode? TransportationMode {
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

    private bool _isLcl;
    [Column("is_lcl")]
    public bool IsLcl {
      get { return _isLcl; }
      set {
        _isLcl = value;
        RaisePropertyChanged(nameof(IsLcl));
      }
    }

    private bool _isLtl;
    [Column("is_ltl")]
    public bool IsLtl {
      get { return _isLtl; }
      set {
        _isLtl = value;
        RaisePropertyChanged(nameof(IsLtl));
      }
    }

    private bool _wantsOriginService;
    [Column("is_ltl")]
    public bool WantsOriginService {
      get { return _wantsOriginService; }
      set {
        _wantsOriginService = value;
        RaisePropertyChanged(nameof(WantsOriginService));
      }
    }

    private bool _wantsDestinationService;
    [Column("is_ltl")]
    public bool WantsDestinationService {
      get { return _wantsDestinationService; }
      set {
        _wantsDestinationService = value;
        RaisePropertyChanged(nameof(WantsDestinationService));
      }
    }

    private DateTime? _cargoReadyDate;
    [Column("cargo_ready_date")]
    public DateTime? CargoReadyDate {
      get { return _cargoReadyDate; }
      set {
        _cargoReadyDate = value;
        RaisePropertyChanged(nameof(CargoReadyDate));
      }
    }

    private DateTime? _targetDeliveryDate;
    [Column("cargo_ready_date")]
    public DateTime? TargetDeliveryDate {
      get { return _targetDeliveryDate; }
      set {
        _targetDeliveryDate = value;
        RaisePropertyChanged(nameof(TargetDeliveryDate));
      }
    }

    // Derived Attributes
    public bool IsShipperBooking {
      get {
        return Shipper?.Id == AppStatics.Singleton.Context.User.Company.Id;
      }
    }
    public bool IsConsigneeBooking {
      get {
        return Consignee?.Id == AppStatics.Singleton.Context.User.Company.Id;
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

    public virtual Location OriginLocation { get; set; }
    [NotMapped]
    public Location OriginLocationBindable {
      get { return OriginLocation; }
      set {
        OriginLocation = value;
        RaisePropertyChanged(nameof(OriginLocationBindable));
      }
    }

    public virtual Port OriginPort { get; set; }
    [NotMapped]
    public Port OriginPortBindable {
      get { return OriginPort; }
      set {
        OriginPort = value;
        RaisePropertyChanged(nameof(OriginPortBindable));
      }
    }

    public override string ToString() {
      return Name;
    }
  }
}

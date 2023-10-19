using System;
using System.Collections.Generic;

using wpf_lib.lib;
using wpf_lib.lib.attributes;

using wpf_generated.functions;


namespace wpf_generated.entities {


  public class Move : EntityBase {

    // Regular Attributes
    private DateTime? _date;
    public DateTime? Date {
      get { return _date; }
      set {
        _date = value;
        RaisePropertyChanged(nameof(Date));
      }
    }

    // Derived Attributes

    // Associations
    public virtual Unit From { get; set; }
    public Unit FromBindable {
      get { return From; }
      set {
        From = value;
        RaisePropertyChanged(nameof(FromBindable));
      }
    }
    public virtual Unit To { get; set; }
    public Unit ToBindable {
      get { return To; }
      set {
        To = value;
        RaisePropertyChanged(nameof(ToBindable));
      }
    }
    public virtual Tenant Tenant { get; set; }
    public Tenant TenantBindable {
      get { return Tenant; }
      set {
        Tenant = value;
        RaisePropertyChanged(nameof(TenantBindable));
      }
    }
  }
}

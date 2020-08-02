using System;
using System.Collections.Generic;

using wpf_lib.lib;
using wpf_lib.lib.attributes;

using wpf_generated.functions;


namespace wpf_generated.entities {


  public class Tenant : EntityBase {

    // Regular Attributes
    private string _name;
    public string Name {
      get { return _name; }
      set {
        _name = value;
        RaisePropertyChanged(nameof(Name));
      }
    }
    private string _phone;
    public string Phone {
      get { return _phone; }
      set {
        _phone = value;
        RaisePropertyChanged(nameof(Phone));
      }
    }
    private string _email;
    public string Email {
      get { return _email; }
      set {
        _email = value;
        RaisePropertyChanged(nameof(Email));
      }
    }

    // Derived Attributes

    // Associations
    public virtual Address PermanentMailingAddress { get; set; }
    public Address PermanentMailingAddressBindable {
      get { return PermanentMailingAddress; }
      set {
        PermanentMailingAddress = value;
        RaisePropertyChanged(nameof(PermanentMailingAddressBindable));
      }
    }

    public static Tenant Create() {
      return new Tenant {
        PermanentMailingAddress = Address.Create(),
      };
    }
  }
}

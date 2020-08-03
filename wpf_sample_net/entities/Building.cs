using System;
using System.Collections.Generic;

using wpf_lib.lib;
using wpf_lib.lib.attributes;
using wpf_sample;

using wpf_generated.functions;


namespace wpf_generated.entities {

  public enum MailboxTypeEnum {
    [Label("Mailroom in Building")]
    InBuilding,
    [Label("Postal System Community Mailbox")]
    Community,
    [Label("Mail Delivered to Unit")]
    Individual,
  }

  public enum PetPolicyEnum {
    NoPets,
    AllPetsOk,
    CatsOnly,
    DogsOnly,
  }


  public class Building : EntityBase {

    // Regular Attributes
    private string _moniker;
    public string Moniker {
      get { return _moniker; }
      set {
        _moniker = value;
        RaisePropertyChanged(nameof(Moniker));
      }
    }
    private string _name;
    public string Name {
      get { return _name; }
      set {
        _name = value;
        RaisePropertyChanged(nameof(Name));
      }
    }
    private string _description;
    public string Description {
      get { return _description; }
      set {
        _description = value;
        RaisePropertyChanged(nameof(Description));
      }
    }
    private DateTime? _dateOfOccupancy;
    public DateTime? DateOfOccupancy {
      get { return _dateOfOccupancy; }
      set {
        _dateOfOccupancy = value;
        RaisePropertyChanged(nameof(DateOfOccupancy));

        RaisePropertyChanged(nameof(AgeInYears));
      }
    }
    private MailboxTypeEnum? _mailboxType;
    public MailboxTypeEnum? MailboxType {
      get { return _mailboxType; }
      set {
        _mailboxType = value;
        RaisePropertyChanged(nameof(MailboxType));
      }
    }
    private PetPolicyEnum? _petPolicy;
    public PetPolicyEnum? PetPolicy {
      get { return _petPolicy; }
      set {
        _petPolicy = value;
        RaisePropertyChanged(nameof(PetPolicy));
      }
    }
    private bool _mailingAddressSameAsPhysical;
    public bool MailingAddressSameAsPhysical {
      get { return _mailingAddressSameAsPhysical; }
      set {
        _mailingAddressSameAsPhysical = value;
        RaisePropertyChanged(nameof(MailingAddressSameAsPhysical));
        RaisePropertyChanged(nameof(ApplicableWhenForMailingAddress));
      }
    }

    // Derived Attributes
    public int? AgeInYears {
      get {
        return AppStatics.Singleton.Context?.Now?.Year - DateOfOccupancy?.Year;
      }
    }
    public bool ApplicableWhenForMailingAddress {
      get {
        return !MailingAddressSameAsPhysical;
      }
    }

    // Associations
    public virtual List<Unit> Units { get; set; }
    public List<Unit> UnitsBindable {
      get { return Units; }
      set {
        Units = value;
        RaisePropertyChanged(nameof(UnitsBindable));
      }
    }
    public virtual Address PhysicalAddress { get; set; }
    public Address PhysicalAddressBindable {
      get { return PhysicalAddress; }
      set {
        PhysicalAddress = value;
        RaisePropertyChanged(nameof(PhysicalAddressBindable));
      }
    }
    public virtual Address MailingAddress { get; set; }
    public Address MailingAddressBindable {
      get { return MailingAddress; }
      set {
        MailingAddress = value;
        RaisePropertyChanged(nameof(MailingAddressBindable));
      }
    }

    // Validations
    public override void CalculateErrors(EntityErrors errors) {
      if (string.IsNullOrWhiteSpace(Name?.ToString()))
        errors.Add("Name is required", nameof(Name));
      if (DateOfOccupancy > AppStatics.Singleton.Context.Now)
        errors.Add("Occupancy date cannot be in the future",
          nameof(DateOfOccupancy));
    }

    public override string ToString() {
      return Name?.ToString();
    }

    public static Building Create() {
      return new Building {
        Moniker = "1",
        MailboxType = MailboxTypeEnum.InBuilding,
        MailingAddressSameAsPhysical = true,
        Units = new List<Unit>(),
        PhysicalAddress = Address.Create(),
        MailingAddress = Address.Create(),
      };
    }
  }
}

using x10_csharp;

using icon;
using lib;

public enum BookingStatus {
  [Icon(Icons.Draft)]
  Draft,

  [Icon(Icons.DollarSign)]
  WaitingForPricing,

  [Icon(Icons.Ticket)]
  [Label("Awaiting Your Review")]
  AwaitingReview,
}

[Description("A pre-<Shipment>. An intention to ship goods containing most of the information necessary to eventually generate a <Shipment>")]
public class Booking {

  // Attributes
  [ReadOnly]
  public string FlexId;

  [PlaceholderText("Description of shipment")]
  [Mandatory]
  public string Name;

  [Mandatory]
  public BookingStatus BookingStatus;

  [Mandatory]
  public TransportationMode TransportationMode;

  [Mandatory]
  public Date CargoReadyDate;

  public Date TargetDeliveryDate;

  [ToolTip("The company entity which should be notified of the arrival of the cargo")]
  [Ui(typeof(TextArea))]
  public string NotifyParty;

  [Mandatory]
  [DefaultValue(false)]
  [Label("Shiopment Type")]
  public bool IsLcl;

  [Mandatory]
  public Incoterms Incoterms;

  [DefaultValue(true)]
  public bool WantsOriginService;

  [DefaultValue(false)]
  public bool WantsExportCustomsService;

  [ToolTip("Please enter a short and concise description of products in this shipment")]
  public string DescriptionOfProducts;
  public bool DescriptionOfProducts_ApplicableWhen() {
    return TransportationMode == TransportationMode.Truck;
  }

  [PlaceholderText("Enter any special instructions, such as handling information")]
  public string SpecialInstructions;

  public double WeightKg;

  public double VolumeCubicM;

  [Mandatory]
  [DefaultValue(Priority.Normal)]
  public Priority Priority;

  // Derived Attributes
  public bool IsShipperBooking() {
    return Shipper == Context.CurrentUser.Company;
  }
  public bool IsConsigneeBooking() {
    return Consignee == Context.CurrentUser.Company;
  }
  public bool requiresChineseProductDescription() {
    return IsShipperBooking() && Shipper.CountryCode == "CN";
  }

  // Associatons
  [Mandatory]
  public Company Shipper;

  [Mandatory]
  public Company Consignee;

  [Mandatory]
  public Many<BookingHsCode> BookingHscodes;
}

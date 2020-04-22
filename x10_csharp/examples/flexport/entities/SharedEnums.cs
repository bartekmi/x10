using x10_csharp;

using icon;

[Description("The type of transportation mode used for the main leg of the journey")]
[Label("Freight Method")]
public enum TransportationMode {
  [Icon(Icons.Airplane)]
  Air,
  [Icon(Icons.Boat)]
  Ocean,
  [Icon(Icons.Truck)]
  Truck,
}

[Description("International standard payment agreement between shipper and consignee")]
public enum Incoterms {
  FOB, EXW, FAS, FCA, CPT, CFR
}

[Description("The type of transportation mode used for the main leg of the journey")]
[Label("Freight Method")]
public enum Priority {
  Normal,
  [Icon(Icons.Lightning)]
  High,
}

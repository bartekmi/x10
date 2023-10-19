using wpf_lib.lib.attributes;

public enum TransportationMode {
  [Icon("boat")]
  Ocean,
  [Icon("airplane")]
  [Label("Fly Fly Away")]
  Air,
  [Icon("truck")]
  Truck,
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpf_sample.lib.attributes;

namespace wpf_sample.entities {
  public enum TransportationMode {
    [Icon("boat")]
    Ocean,
    [Icon("airplane")]
    Air,
    [Icon("truck")]
    Truck,
  }
}

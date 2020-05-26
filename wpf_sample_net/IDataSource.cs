using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpf_sample.entities.booking;

namespace wpf_sample {
  public interface IDataSource {
    IEnumerable<Company> Companies { get; }
    IEnumerable<Location> Locations { get; }
    IEnumerable<Port> Ports { get; }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using wpf_generated.entities;
using wpf_sample.entities.booking;
using wpf_sample.entities.core;

namespace wpf_sample {
  public interface IDataSource {
    IEnumerable<Company> Companies { get; }
    IEnumerable<Location> Locations { get; }
    IEnumerable<Port> Ports { get; }

    void Create(Booking model);
    void Create(Building model);
  }
}

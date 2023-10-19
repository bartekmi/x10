using System;
using System.Collections.Generic;
using System.Linq;

using wpf_lib.lib;
using wpf_generated.entities;
using wpf_sample.entities.booking;
using wpf_sample.entities.core;

namespace wpf_sample {
  public interface IDataSource {
    // Large example stuff
    IEnumerable<Company> Companies { get; }
    IEnumerable<Location> Locations { get; }
    IEnumerable<Port> Ports { get; }

    // Small example stuff
    IEnumerable<Building> Buildings { get; }

    void CreateOrUpdate(Booking model);
    void CreateOrUpdate(Building model);

    T GetById<T>(int id) where T : EntityBase;
  }
}

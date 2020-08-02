using System.Collections.Generic;

using wpf_generated.entities;

using wpf_sample.entities.booking;
using wpf_sample.entities.core;

namespace wpf_sample {
  public class DataSourceInMemory : IDataSource {
    private List<Company> _companies;
    public IEnumerable<Company> Companies { get { return _companies; } }

    private List<Location> _locations;
    public IEnumerable<Location> Locations { get { return _locations; } }

    private List<Port> _ports;
    public IEnumerable<Port> Ports { get { return _ports; } }

    public IEnumerable<Booking> Bookings { get; } = new List<Booking>();
    public IEnumerable<Building> Buildings { get; } = new List<Building>();

    public DataSourceInMemory() {
      _companies = new List<Company>() {
        new Company() {
          Id = 1,
          LegalName = "Sapphire Software Inc.",
          Address = "115 Scenic Park Rd. NW",
        },
        new Company() {
          Id = 2,
          LegalName = "Acme, Inc.",
          Address = "179 Acme Rd. NW",
        },
        new Company() {
          Id = 3,
          LegalName = "Safeway Ltd.",
          Address = "1115 Food St. SE",
        },
      };

      _locations = new List<Location>() {
        new Location() {
          Id = 1,
          Name = "Acme Beijing Warehouse",
          City = "Beijing",
          StateOrProvince = "Beijing District",
          CountryCode = "CN",
        },
        new Location() {
          Id = 2,
          Name = "Safeway Story #28",
          City = "Calgary",
          StateOrProvince = "Alberta",
          CountryCode = "CA",
        },
        new Location() {
          Id = 3,
          Name = "Safeway Story #72",
          City = "Edmonton",
          StateOrProvince = "Alberta",
          CountryCode = "CA",
        },
      };

      _ports = new List<Port>() {
        new Port() {
          Id = 1,
          Name = "Port of Guangdong",
          City = "Guangdong",
          CountryCode = "CN",
        },
        new Port() {
          Id = 2,
          Name = "Los Angeles Port Authority",
          City = "Los Angeles",
          StateOrProvince = "CA",
          CountryCode = "US",
        },
        new Port() {
          Id = 3,
          Name = "Seatle Port Authority",
          City = "Seattle",
          StateOrProvince = "WA",
          CountryCode = "US",
        },
      };
    }

    public void Create(Booking booking) {
      ((List<Booking>)Bookings).Add(booking);
    }

    public void Create(Building building) {
      ((List<Building>)Buildings).Add(building);
    }
  }
}

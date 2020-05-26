using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpf_sample.entities.booking;

namespace wpf_sample {
  public class DataSourceInMemory : IDataSource {
    private List<Company> _companies;
    public IEnumerable<Company> Companies { get { return _companies; } }

    private List<Location> _locations;
    public IEnumerable<Location> Locations { get { return _locations; } }

    private List<Port> _ports;
    public IEnumerable<Port> Ports { get { return _ports; } }

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
  }
}

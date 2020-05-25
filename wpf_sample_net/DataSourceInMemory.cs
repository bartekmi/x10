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
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using wpf_lib;

using wpf_generated.data;
using wpf_generated.entities;

namespace wpf_generated {
  public class AppStatics : BaseAppStatics {
    public static AppStatics Singleton { get { return (AppStatics)BaseSingleton; } }

    public static void Create() {
      BaseSingleton = new AppStatics();
    }

    public IDataSource DataSource { get; private set; }
    public __Context__ Context { get; set; }

    private AppStatics() {
      DataSource = new DataSourceInMemory();
      WpfLibConfig.AssemblyForFindingTypes = "wpf_generated";
    }
  }
}

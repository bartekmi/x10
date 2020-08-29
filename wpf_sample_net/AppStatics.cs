using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpf_lib;
using wpf_lib.storybook;
using wpf_sample.entities;

namespace wpf_sample {
  public class AppStatics : BaseAppStatics {
    public static AppStatics Singleton { 
      get {
        return (AppStatics)BaseSingleton;
      }
    }

    public static void Create() {
      BaseSingleton = new AppStatics();
    }

    public IDataSource DataSource { get; private set; }
    public __Context__ Context { get; set; }

    private AppStatics() {
      DataSource = new DataSourceInMemory();
      WpfLibConfig.AssemblyForFindingTypes = "wpf_sample_net";
    }
  }
}

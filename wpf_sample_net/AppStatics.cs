﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpf_lib;
using wpf_sample.entities;

namespace wpf_sample {
  public class AppStatics {
    private static AppStatics _singleton;
    public static AppStatics Singleton { 
      get {
        if (_singleton == null)
          _singleton = new AppStatics();
        return _singleton;
      }
    }

    public IDataSource DataSource { get; private set; }
    public __Context__ Context { get; set; }


    private AppStatics() {
      DataSource = new DataSourceInMemory();
      WpfLibConfig.AssemblyForFindingTypes = "wpf_sample_net";
    }
  }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpf_lib;
using wpf_lib.storybook;

namespace wpf_lib {
  public abstract class BaseAppStatics {
    public static BaseAppStatics BaseSingleton { get; protected set; }

    public INavigation Navigation { get; set; }

    protected BaseAppStatics() {
      // Do nothing
    }
  }
}

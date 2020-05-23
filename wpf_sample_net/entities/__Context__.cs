using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpf_sample.entities.booking;

namespace wpf_sample.entities {
  public class __Context__ {
    public static __Context__ Singleton { get; set; }

    public User User { get; set; }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_lib {
  public static class WpfLibConfig {
    public static string AssemblyForFindingTypes { get; set; }

    internal static Type GetType(string externalTypeName) {
      return Type.GetType(string.Format("{0}, {1}",
        externalTypeName, AssemblyForFindingTypes));
    }
  }
}

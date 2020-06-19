using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_generated.functions {
  public static class Functions {
    public static string ToHuman(DateTime? date) {
      if (date == null)
        return "-";
      return date.Value.ToString("MMM d");
    }
  }
}

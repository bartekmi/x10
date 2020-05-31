using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_sample.lib.attributes {
  public class LabelAttribute : Attribute {
    private string _label;
    public LabelAttribute(string label) {
      _label = label;
    }
  }
}

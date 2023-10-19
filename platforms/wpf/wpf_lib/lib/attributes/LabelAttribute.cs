using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_lib.lib.attributes {
  public class LabelAttribute : Attribute {
    public string Label { get; private set; }

    public LabelAttribute(string label) {
      Label = label;
    }
  }
}

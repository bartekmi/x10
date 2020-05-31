using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_sample.lib.attributes {
  public class IconAttribute : Attribute {
    private string _icon;
    public IconAttribute(string icon) {
      _icon = icon;
    }
  }
}

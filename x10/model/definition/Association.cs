using System;
using System.Collections.Generic;
using System.Text;

namespace x10.model.definition {
  public class Association : Member {
    public bool IsMany { get; set; }
    public bool Owns { get; set; }
  }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace x10.formula {
  public class ExpBinary : ExpBase {
    public string Token { get; set; }
    public ExpBase Left { get; set; }
    public ExpBase Right { get; set; }
  }
}

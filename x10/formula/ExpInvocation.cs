using System;
using System.Collections.Generic;
using System.Text;

namespace x10.formula {
  public class ExpInvocation : ExpBase {
    public string FunctionName { get; set; }
    public List<ExpBase> Arguments { get; set; }
  }
}

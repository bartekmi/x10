using System;
using System.Collections.Generic;
using System.Text;

namespace x10.formula {
  public class ExpMemberAccess : ExpBase {
    public ExpBase Expression { get; set; }
    public String MemberName { get; set; }
  }
}

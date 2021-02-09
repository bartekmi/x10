using System;
using System.Collections.Generic;
using System.Text;

using x10.model.definition;

namespace x10.gen.sql.primitives {
  public class MemberAndValue {
    public Member Member;
    public object Value;

    public override String ToString() {
      return string.Format("{0}: {1}", Member, Value);
    }
  }
}

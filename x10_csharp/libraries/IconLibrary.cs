using System;
using System.Collections.Generic;
using System.Text;

namespace icon {
  public enum Icons {
    Airplane,
    Boat,
    Draft,
    DollarSign,
    Lightning,
    Ticket,
    Truck,
  }

  // Wish there was an option for enum value/member, but thre isn't
  // https://stackoverflow.com/questions/5032774/what-attributetarget-should-i-use-for-enum-members
  [AttributeUsage(AttributeTargets.Field)]
  public class Icon : Attribute {
    public Icons Value { get; private set; }

    public Icon(Icons value) {
      Value = value;
    }
  }
}

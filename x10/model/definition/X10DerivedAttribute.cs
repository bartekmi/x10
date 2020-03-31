using System;
using System.Collections.Generic;
using System.Text;

using x10.model.metadata;

namespace x10.model.definition {
  public class X10DerivedAttribute : X10Attribute {
    public string Formula { get; set; }

    public X10DerivedAttribute() {
      // Derived attributes are always read-only, and this cannot be changed
      // in the Entity definition files.
      IsReadOnly = true;
    }
  }
}

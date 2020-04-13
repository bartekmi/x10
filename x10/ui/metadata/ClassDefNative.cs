using System;
using System.Collections.Generic;
using System.Text;


using x10.model.metadata;

namespace x10.ui.metadata {
  public class ClassDefNative : ClassDef {
    public string ImportParth { get; set; }
    public string HelpUrl { get; set; }

    public ClassDefNative() : base(new UiAttributeDefinition[0]) {
      // Do nothing
    }

    public ClassDefNative(IEnumerable<UiAttributeDefinition> attrDefinitions) : base(attrDefinitions) {
      // Do nothing
    }
  }
}

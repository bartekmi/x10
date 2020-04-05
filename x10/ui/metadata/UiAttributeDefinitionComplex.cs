using System;
using System.Collections.Generic;
using System.Text;

namespace x10.ui.metadata {
  public class UiAttributeDefinitionComplex : UiAttributeDefinition {
    // Every complex attribute definition (enalogous to an attribute in WPF
    // which has to be defined with the <Class.Attribute> syntax) is defined
    // by a non-UI UiDefinition object
    public UiDefinitionNative UiDefinition { get; set; }
  }
}

using System;
using System.Collections.Generic;
using System.Text;

using x10.model.metadata;
using x10.parsing;
using x10.ui.composition;

namespace x10.ui.metadata {
  public class UiAttributeDefinitionAtomic : UiAttributeDefinition {
    public DataType DataType { get; set; }

    public UiAttributeValueAtomic CreateValueAndAddToOwnerAtomic(IAcceptsUiAttributeValues owner, XmlBase xmlBase) {
      return (UiAttributeValueAtomic)CreateValueAndAddToOwner(owner, xmlBase);
    }

  }
}

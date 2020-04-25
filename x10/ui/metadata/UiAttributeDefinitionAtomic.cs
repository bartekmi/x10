using System;
using System.Collections.Generic;
using System.Text;

using x10.model.metadata;
using x10.parsing;
using x10.ui.composition;

namespace x10.ui.metadata {
  public class UiAttributeDefinitionAtomic : UiAttributeDefinition {
    public DataType DataType { get; set; }

    // Same meaning as in WPF - an Attached Attribute is defined by a class, but is
    // mean to be used by other classes. The canonical case is how Grid defines
    // row/column to be used by children
    public bool IsAttached { get; set; }

    public UiAttributeValueAtomic CreateValueAndAddToOwnerAtomic(IAcceptsUiAttributeValues owner, XmlBase xmlBase) {
      return (UiAttributeValueAtomic)CreateValueAndAddToOwner(owner, xmlBase);
    }

  }
}

﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using x10.parsing;
using x10.ui.metadata;

namespace x10.ui.composition {
  public class UiAttributeValueAtomic : UiAttributeValue {
    public object Value { get;set; }

    public UiAttributeValueAtomic(UiAttributeDefinitionAtomic attrDefinition, IAcceptsUiAttributeValues owner, XmlBase xmlBase) 
      : base(attrDefinition, owner, xmlBase) {
      // Do nothing
    }

    public void Print(TextWriter writer) {
      if (Definition.Name == ParserXml.ELEMENT_NAME)
        return;

      writer.Write(" ");
      writer.Write("{0}='{1}'", Definition.Name, Value);
    }

    public override string ToString() {
      return string.Format("{0}='{1}'", Definition?.Name, Value);
    }
  }
}

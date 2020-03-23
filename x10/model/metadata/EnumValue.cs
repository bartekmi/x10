using System;
using System.Collections.Generic;

// See note in DataType class. This class may end up being moved to 'definition'
using x10.model.definition;

namespace x10.model.metadata {
  public class EnumValue : IAcceptsModelAttributeValues {
    public object Value { get; set; }
    public string Label { get; set; }
    public string IconName { get; set; }

    public List<ModelAttributeValue> AttributeValues { get; private set; }

    public EnumValue() {
      AttributeValues = new List<ModelAttributeValue>();
    }
  }
}
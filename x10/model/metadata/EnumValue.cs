using System;
using System.Collections.Generic;

using x10.utils;
using x10.parsing;
// See note in DataType class. This class may end up being moved to 'definition'
using x10.model.definition;

namespace x10.model.metadata {
  public class EnumValue : IAcceptsModelAttributeValues {
    public object Value { get; set; }
    public string Label { get; set; }
    public string IconName { get; set; }

    // Derived
    public string ValueUpperCased {
      get { return NameUtils.CapitalizeFirstLetter(Value.ToString()); }
    }

    // IAcceptsModelAttributeValues
    public List<ModelAttributeValue> AttributeValues { get; private set; }
    public TreeElement TreeElement { get; set; }

    public EnumValue() {
      AttributeValues = new List<ModelAttributeValue>();
    }

    public EnumValue(object value) : this() {
      if (value == null)
        throw new Exception("Value cannot be null");
      Value = value;
    }

    public override string ToString() {
      return Value.ToString();
    }
  }
}
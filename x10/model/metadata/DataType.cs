using System;
using System.Collections.Generic;
using System.Linq;

using x10.parsing;

// This unpleasant circular dependency is due to the fact that DataType does double-dute as
// our basic data types, and also enums defined in the 'definition' sister package
// Consider deriving new type EnumDataType from DataType which would live in the 'definition' package
using x10.model.definition;

namespace x10.model.metadata {
  public class DataType {
    public string Name { get; set; }
    public string Description { get; set; }
    public Func<string, object> ParseFunction { get; set; }
    public string Examples { get; set; }

    public DataType() {
      // Do nothing
    }

    public object Parse(string text) {
      try {
        return ParseFunction(text);
      } catch {
        return null;
      }
    }

    public override string ToString() {
      return "DataType: " + Name;
    }

    // Enum-related functions. At some point, we may extract an derived class for this
    public bool IsEnum { get { return this is DataTypeEnum; } }
  }
}
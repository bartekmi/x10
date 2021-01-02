using System;
using System.Collections.Generic;
using System.Linq;

using x10.parsing;

// This unpleasant circular dependency is due to the fact that DataType does double-duty as
// our basic data types, and also enums defined in the 'definition' sister package
// Consider deriving new type EnumDataType from DataType which would live in the 'definition' package
using x10.model.definition;

namespace x10.model.metadata {
  public class DataTypeEnum : DataType, IAcceptsModelAttributeValues {
    public List<EnumValue> EnumValues { get; private set; }
    public string UiName { get; set; }
    public bool IsDefinedInEnumsFile { get; set; }

    // IAcceptsModelAttributeValues
    public List<ModelAttributeValue> AttributeValues { get; private set; }
    public TreeElement TreeElement { get; set; }

    // Dervied
    public IEnumerable<string> AvailableValuesAsStrings { get { return EnumValues.Select(x => x.Value.ToString()); } }

    public DataTypeEnum() {
      EnumValues = new List<EnumValue>();
      AttributeValues = new List<ModelAttributeValue>();
      ParseFunction = (text) => ParseEnum(text);
    }

    public DataTypeEnum(IEnumerable<object> enumValues) : this() {
      EnumValueValues = enumValues;
    }

    private ParseResult ParseEnum(string text) {
      if (HasEnumValue(text))
        return new ParseResult(text);

      string error = string.Format("'{0}' is not a valid member of the Enumerated Type '{1}'. Valid values are: {2}.",
        text, Name, string.Join(", ", EnumValueValues.OrderBy(x => x)));

      return new ParseResult(null) {
        ParseErrorMessage = error,
      };
    }

    // Enum-related functions. At some point, we may extract an derived class for this
    public IEnumerable<object> EnumValueValues {
      get {
        return EnumValues.Select(x => x.Value);
      }
      set {
        EnumValues = value.Select(x => new EnumValue(x)).ToList();
      }
    }

    public bool HasEnumValue(object value) {
      return EnumValues.Any(x => x.Value.Equals(value));
    }

    public EnumValue FindEnumValue(object value) {
      // Using first instead of single to avoid exceptions
      // Enum compilation will catch duplicates
      return EnumValues.FirstOrDefault(x => x.Value.Equals(value));
    }

    public override string ToString() {
      return "DataType: " + Name;
    }
  }
}
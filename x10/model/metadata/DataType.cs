using System;
using System.Collections.Generic;
using System.Linq;

using x10.parsing;

// This unpleasant circular dependency is due to the fact that DataType does double-dute as
// our basic data types, and also enums defined in the 'definition' sister package
// Consider deriving new type EnumDataType from DataType which would live in the 'definition' package
using x10.model.definition;

namespace x10.model.metadata {
  public class DataType : IAcceptsModelAttributeValues {
    public string Name { get; set; }
    public string Description { get; set; }
    public List<EnumValue> EnumValues { get; private set; }
    public Func<string, object> ParseFunction { get; set; }
    public string Examples { get; set; }

    // IAcceptsModelAttributeValues
    public List<ModelAttributeValue> AttributeValues { get; private set; }
    public TreeElement TreeElement { get; set; }

    public DataType() {
      EnumValues = new List<EnumValue>();
      AttributeValues = new List<ModelAttributeValue>();
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
  }

  public class DataTypes {

    private static DataTypes _singleton;
    public static DataTypes Singleton {
      get {
        if (_singleton == null)
          _singleton = new DataTypes();
        return _singleton;
      }
    }

    public readonly DataType Integer;
    public readonly DataType Float;
    public readonly DataType String;
    public readonly DataType Boolean;
    public readonly DataType Date;
    public readonly DataType Timestamp;

    public readonly List<DataType> All;

    private DataTypes() {

      // This list contains all the "regular" data types: Integer, String, etc
      All = new List<DataType>() {
        new DataType() {
          Name = "Integer",
          Description = "Counting numbers, both positive and negative: e.g. 1, 2, 3..., -7, 1024",
          ParseFunction = (s) => int.Parse(s),
          Examples = "1, 7, -8",
        },
        new DataType() {
          Name = "Float",
          Description = "Any number, including fractional numbers with decimal, both positive and negative",
          ParseFunction = (s) => double.Parse(s),
          Examples = "12.3, -0.00777, 1.78e-12",
        },
        new DataType() {
          Name = "String",
          Description = "Text - long or short",
          ParseFunction = (s) => s,
        },
        new DataType() {
          Name = "Boolean",
          Description = "True or false",
          ParseFunction = (s) => bool.Parse(s),
          Examples = "True, False",
        },
        new DataType() {
          Name = "Date",
          Description = "A calendar date",
          ParseFunction = (s) => DateTime.Parse(s).Date,
          Examples = "2020-01-31"
        },
        new DataType() {
          Name = "Timestamp",
          Description = "A unique point in time, expressed in UTC time",
          ParseFunction = (s) => DateTime.Parse(s),
        },
        new DataType() {
          Name = "Money",
          Description = "Fixed-point currency",
          ParseFunction = (s) => Double.Parse(s),
          Examples = "12.30, -208.12, 0",
        },
      };


      // Constants for the "regular" data types
      Integer = Find("Integer");
      Float = Find("Float");
      String = Find("String");
      Boolean = Find("Boolean");
      Date = Find("Date");
      Timestamp = Find("Timestamp");
    }

    public DataType Find(string dataTypeName) {
      DataType type = All.SingleOrDefault(x => x.Name == dataTypeName);

      // TODO This should be encapsulated in something like AllEntities. Benefits:
      // 1. Speed
      // 2. Better error robustness - e.g. what to do when type is duplicated
      if (type == null)
        type = ModelEnums.FirstOrDefault(x => x.Name == dataTypeName);
      return type;
    }

    public void AddDataType(DataType customDataType) {
      All.Add(customDataType);
    }

    public List<DataType> ModelEnums = new List<DataType>();
    public void AddModelEnum(DataType modelDataType) {
      // TODO: ensure this is not a duplicate of a built-in type
      ModelEnums.Add(modelDataType);
    }
  }
}
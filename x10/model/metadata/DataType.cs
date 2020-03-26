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

    public readonly DataType DataType;
    public readonly DataType SameAsDataType;
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
      };

      // These are the "Special" data types
      DataType = new DataType() {
        Name = "DataType",
        Description = "A 'Meta' data-type - expects the name of a Data Type",
        ParseFunction = (s) => {
          return All.FirstOrDefault(x => x.Name == s);
        },
        Examples = string.Join(", ", All.Select(x => x.Name)),
      };
      SameAsDataType = new DataType();

      // Constants for the "regular" data types
      Integer = Find("Integer");
      Float = Find("Float");
      String = Find("String");
      Boolean = Find("Boolean");
      Date = Find("Date");
      Timestamp = Find("Timestamp");
    }

    public DataType Find(string dataTypeName) {
      return All.SingleOrDefault(x => x.Name == dataTypeName);
    }

    public void AddDataType(DataType customDataType) {
      All.Add(customDataType);
    }

    public List<DataType> ModelEnums = new List<DataType>();
    public void AddModelEnum(DataType modelDataType) {
      ModelEnums.Add(modelDataType);
    }
  }
}
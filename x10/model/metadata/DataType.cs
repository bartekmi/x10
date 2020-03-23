using System;
using System.Collections.Generic;
using System.Linq;

namespace x10.model.metadata {
  public class DataType {
    public string Name { get; set; }
    public string Description { get; set; }
    public Object[] EnumValues { get; set; }
    public Func<string, object> ParseFunction { get; set; }
    public string Examples { get; set; }

    public object Parse(string text) {
      try {
        return ParseFunction(text);
      } catch {
        return null;
      }
    }
  }

  public static class DataTypes {

    // These are the "Special" data types
    public readonly static DataType DataType = new DataType() {
      Name = "DataType",
      Description = "A 'Meta' data-type - expects the name of a Data Type",
      ParseFunction = (s) => {
        return All.FirstOrDefault(x => x.Name == s);
      },
      Examples = string.Join(", ", All.Select(x => x.Name)),
    };
    public readonly static DataType SameAsDataType = new DataType();

    // Constants for the "regular" data types
    public readonly static DataType Integer = Find("Integer");
    public readonly static DataType Float = Find("Float");
    public readonly static DataType String = Find("String");
    public readonly static DataType Boolean = Find("Boolean");
    public readonly static DataType Date = Find("Date");
    public readonly static DataType Timestamp = Find("Timestamp");


    // This list contains all the "regular" data types: Integer, String, etc
    public static List<DataType> All = new List<DataType>() {
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

    public static DataType Find(string dataTypeName) {
      return All.SingleOrDefault(x => x.Name == dataTypeName);
    }

    public static void AddDataType(DataType customDataType) {
      All.Add(customDataType);
    }
  }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace x10.model.metadata {

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
    public readonly DataType Money;
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
          ParseFunction = (s) => new ParseResult(int.Parse(s)),
          Examples = "1, 7, -8",
        },
        new DataType() {
          Name = "Float",
          Description = "Any number, including fractional numbers with decimal, both positive and negative",
          ParseFunction = (s) => new ParseResult(double.Parse(s)),
          Examples = "12.3, -0.00777, 1.78e-12",
        },
        new DataType() {
          Name = "String",
          Description = "Text - long or short",
          ParseFunction = (s) => new ParseResult(s),
        },
        new DataType() {
          Name = "Boolean",
          Description = "True or false",
          ParseFunction = (s) => new ParseResult(bool.Parse(s)),
          Examples = "True, False",
        },
        new DataType() {
          Name = "Date",
          Description = "A calendar date",
          ParseFunction = (s) => new ParseResult(DateTime.Parse(s).Date),
          Examples = "2020-01-31",
          PropertiesInit = () => new List<DataTypeProperty>() {
            new DataTypeProperty("year", Integer),
            new DataTypeProperty("dayOfMonth", Integer),
            new DataTypeProperty("monthName", String),
          }
        },
        new DataType() {
          Name = "Timestamp",
          Description = "A unique point in time, expressed in UTC time",
          ParseFunction = (s) => new ParseResult(DateTime.Parse(s)),
          PropertiesInit = () => new List<DataTypeProperty>() {
            new DataTypeProperty("year", Integer),
            new DataTypeProperty("dayOfMonth", Integer),
            new DataTypeProperty("monthName", String),
          }
        },
        new DataType() {
          Name = "Money",
          Description = "Fixed-point currency",
          ParseFunction = (s) => new ParseResult(Double.Parse(s)),
          Examples = "12.30, -208.12, 0",
        },
      };


      // Constants for the "regular" data types
      Boolean = Find("Boolean");
      Date = Find("Date");
      Float = Find("Float");
      Integer = Find("Integer");
      Money = Find("Money");
      String = Find("String");
      Timestamp = Find("Timestamp");

      foreach (DataType dataType in All)
        dataType.Initialize();
    }

    public DataType Find(string dataTypeName) {
      return All.SingleOrDefault(x => x.Name == dataTypeName);
    }

    public void AddDataType(DataType customDataType) {
      All.Add(customDataType);
    }

    public void AddDataTypes(IEnumerable<DataType> customDataTypes) {
      foreach (DataType dataType in customDataTypes)
        AddDataType(dataType);
    }
  }
}

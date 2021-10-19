using System;
using System.Collections.Generic;
using System.Linq;

using x10.model.definition;

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
    public readonly DataType Color;

    public static readonly DataType ERROR = new DataType() { Name = "ERROR" };

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
          PropertiesInit = () => new Entity() {
            Name = "DataTypeDate",
            LocalMembers = new List<Member>() {
              new X10RegularAttribute() {
                Name = "year",
                DataType = Integer,
              },
              new X10RegularAttribute() {
                Name = "dayOfMonth",
                DataType = Integer,
              },
              new X10RegularAttribute() {
                Name = "monthName",
                DataType = String,
              },
            },
          }
        },
        new DataType() {
          Name = "Timestamp",
          Description = "A unique point in time, expressed in UTC time",
          ParseFunction = (s) => new ParseResult(DateTime.Parse(s)),
          PropertiesInit = () => new Entity() {
            Name = "DataTypeDate",
            LocalMembers = new List<Member>() {
              new X10RegularAttribute() {
                Name = "year",
                DataType = Integer,
              },
              new X10RegularAttribute() {
                Name = "dayOfMonth",
                DataType = Integer,
              },
              new X10RegularAttribute() {
                Name = "monthName",
                DataType = String,
              },
              new X10RegularAttribute() {
                Name = "date",
                DataType = Date,
              },
            },
          }
        },
        new DataType() {
          Name = "Money",
          Description = "Fixed-point currency",
          ParseFunction = (s) => new ParseResult(Double.Parse(s)),
          Examples = "12.30, -208.12, 0",
        },
        new DataType() {
          Name = "Color",
          Description = "A user-interface color, expressed by name or hex value",
          ParseFunction = (s) => new ParseResult(ParseColor(s)),
          Examples = "#09C, #0099CC, white, silver gray, black, red (see https://en.wikipedia.org/wiki/Web_colors)",
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
      Color = Find("Color");

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

    private readonly string[] COLOR_NAMES = 
      new string[] {"white", "silver", "gray", "black", "red", "maroon", "yellow", "olive", "lime", "green", "aqua", "teal", "blue", "navy", "fuchsia", "purple"};

    private string ParseColor(string color) {
      color = color.ToLower();

      if (color.StartsWith("#")) {
        string raw = color.Substring(1);
        if (raw.Length == 3 || raw.Length == 6)
          foreach(char c in raw)
            ValidateHexDigit(color, c);
        else
          throw new Exception("Color which begins with '#' must be followed by exactly 3 or 6 characters");
      } else {
        if (!COLOR_NAMES.Contains(color)) 
          throw new Exception(string.Format("{0} is not a valid color. Valid colors are: {1}", 
            color, string.Join(", ", COLOR_NAMES)));
      }

      return color;
    }

    private void ValidateHexDigit(string color, char c) {
      if (char.IsDigit(c) || c >= 'a' && c <= 'f')
        return;

      throw new Exception(string.Format("Hash (#) Color expression {0} must only contain hexadecimal characters: 0-9 and a-f (case insensitive)", color));
    }
  }
}

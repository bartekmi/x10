using System;
using System.Collections.Generic;
using System.Linq;

using x10.parsing;
using x10.model.metadata;
using x10.model.definition;

namespace x10.model {
  public class AllEnums {
    // The reason the values are a list is to account for problems where multiple 
    // enums with the same name have been defined 
    private readonly Dictionary<string, List<DataTypeEnum>> _enumsByName
      = new Dictionary<string, List<DataTypeEnum>>();
    private readonly MessageBucket _messages;

    public AllEnums(MessageBucket messages) {
      _messages = messages;
    }

    public void Add(DataTypeEnum anEnum) {
      if (!_enumsByName.TryGetValue(anEnum.Name, out List<DataTypeEnum> enums)) {
        enums = new List<DataTypeEnum>();
        _enumsByName[anEnum.Name] = enums;
      }
      enums.Add(anEnum);
    }

    public IEnumerable<DataTypeEnum> All {
      get { return _enumsByName.Values.SelectMany(x => x); }
    }

    internal DataTypeEnum FindEnumErrorIfMultiple(string typeName, IParseElement parseElement) {
      if (!_enumsByName.TryGetValue(typeName, out List<DataTypeEnum> enums))
        return null;

      if (enums.Count > 1) {
        _messages.AddError(parseElement,
          string.Format("Multiple Enums with the name '{0}' exist", typeName));
        return null;
      }

      return enums.Single();
    }

    public DataType FindDataTypeByNameWithError(string typeName, IParseElement parseElement, bool doErrors = true) {

      DataType builtInDataType = DataTypes.Singleton.Find(typeName);
      if (builtInDataType != null)
        return builtInDataType;

      // Check if enum exists
      if (!_enumsByName.TryGetValue(typeName, out List<DataTypeEnum> enums)) {
        if (doErrors)
          _messages.AddError(parseElement,
            string.Format("Neither Enum nor a built-in data tyes '{0}' is defined", typeName));
        return null;
      }

      if (enums.Count > 1) {
        if (doErrors)
          _messages.AddError(parseElement,
          string.Format("Multiple Enums with the name '{0}' exist", typeName));
        return null;
      }

      return enums.Single();
    }

    internal DataType FindDataTypeByName(string typeName) {
      return FindDataTypeByNameWithError(typeName, null, false);
    }
  }
}

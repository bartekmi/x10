using System;
using System.Collections.Generic;
using System.Linq;

using x10.parsing;
using x10.model.metadata;
using x10.model.definition;

namespace x10.model {

  #region Related Classes
  // Couldn't decide where this belongs
  public class Argument : IAcceptsModelAttributeValues {
    public string Name { get; set; }
    public string Description { get; set; }
    public DataType Type { get; set; }

    // IAcceptsModelAttributeValues
    public List<ModelAttributeValue> AttributeValues { get; private set; }
    public TreeElement TreeElement { get; set; }

    public Argument() {
      AttributeValues = new List<ModelAttributeValue>();
    }
  }
  public class Function : IAcceptsModelAttributeValues {
    public string Name { get; set; }
    public string Description { get; set; }
    public DataType ReturnType { get; set; }
    public List<Argument> Arguments { get; set; }

    // IAcceptsModelAttributeValues
    public List<ModelAttributeValue> AttributeValues { get; private set; }
    public TreeElement TreeElement { get; set; }

    public Function() {
      AttributeValues = new List<ModelAttributeValue>();
      Arguments = new List<Argument>();
    }
  }
  #endregion

  public class AllFunctions {
    // The reason the values are a list is to account for problems where multiple 
    // enums with the same name have been defined 
    private readonly Dictionary<string, List<Function>> _functionsByName 
      = new Dictionary<string, List<Function>>();
    private readonly MessageBucket _messages;

    public AllFunctions(MessageBucket messages) {
      _messages = messages;
    }

    public void Add(Function anEnum) {
      if (!_functionsByName.TryGetValue(anEnum.Name, out List<Function> functions)) {
        functions = new List<Function>();
        _functionsByName[anEnum.Name] = functions;
      }
      functions.Add(anEnum);
    }

    public IEnumerable<Function> All { 
      get { return _functionsByName.Values.SelectMany(x => x);  } 
    }

    internal Function FindFunctionErrorIfMultiple(string name, IParseElement parseElement) {
      if (!_functionsByName.TryGetValue(name, out List<Function> functions)) 
        return null;

      if (functions.Count > 1) {
        _messages.AddError(parseElement,
          string.Format("Multiple Functions with the name '{0}' exist", name));
        return null;
      }

      return functions.Single();
    }
  }
}

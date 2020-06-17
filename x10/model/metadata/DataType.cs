using System;
using System.Collections.Generic;
using System.Linq;

using x10.parsing;

// This unpleasant circular dependency is due to the fact that DataType must have a default
// UI element if none is specified by Member or InstanceModelReference
// One solution would be to have a separate dictionary where this can be looked up
using x10.ui.metadata;

namespace x10.model.metadata {

  public class ParseResult {
    // The result of the parsing operation. If null, this would indicate a parse failure.
    public object Result;

    // Optionally, the DataType ParseFunction may provide a type-specific perse error 
    // message. If this is omitted, a generic error message will be created.
    public string ParseErrorMessage;

    public ParseResult(object result) {
      Result = result;
    }
  }


  public class DataType {
    public string Name { get; set; }
    public string Description { get; set; }
    public Func<string, ParseResult> ParseFunction { get; set; }
    public string Examples { get; set; }


    public DataType() {
      // Do nothing
    }

    public object Parse(string text, MessageBucket messages, IParseElement element, string attributeName) {
      try {
        ParseResult result = ParseFunction(text);

        // A null parese result indicates a parse failure. A suitable message may or may not have been provided.
        if (result.Result == null)
          AddParseError(text, messages, element, attributeName, result.ParseErrorMessage);

        return result.Result;
      } catch {
        AddParseError(text, messages, element, attributeName, null);
        return null;
      }
    }

    private void AddParseError(string text, MessageBucket messages, IParseElement element, string attributeName, string errorMessage) {
      if (errorMessage == null)
        errorMessage = string.Format("could not parse a(n) {0} from '{1}'. Examples of valid data of this type: {2}.",
          Name, text, Examples);

      string completeMessage = string.Format("Error parsing attribute '{0}': {1}",
        attributeName, errorMessage);

      messages.AddError(element, completeMessage);
    }

    public override string ToString() {
      return Name;
    }

    // Enum-related functions. At some point, we may extract an derived class for this
    public bool IsEnum { get { return this is DataTypeEnum; } }
  }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using x10.parsing;

namespace x10.model {
  public static class ModelValidationUtils {

    public const string CONTEXT_ENTITY_NAME = "__Context__";

    private enum Style {
      UpperCamelCase,               // likeThis
      LowerCamelCase,               // LikeThis
      SnakeCase,                    // like_this

      LowerCamelCaseOrAllCaps,      // likeThis or LIKETHIS or LIKE_THIS
      SnakeCaseOrLowerCamelCase,    // like_this or likeThis
    }

    private readonly static Regex UPPER_CASE_CAMEL_REGEX = new Regex("^[A-Z][a-zA-Z0-9]*$");
    private readonly static Regex LOWER_CASE_CAMEL_REGEX = new Regex("^[a-z][a-zA-Z0-9]*$");
    private readonly static Regex ALL_CAPS = new Regex("^[A-Z][A-Z0-9_]*$");
    private readonly static Regex SNAKE_CASE = new Regex("^[a-z]+(_[a-z]+)*$");

    public static bool ValidateEntityName(string entityName, IParseElement element, MessageBucket messages) {
      if (entityName == CONTEXT_ENTITY_NAME)
        return true;
      string examples = "'User', 'PurchaseOrder'";
      return Validate(Style.UpperCamelCase, entityName, "Entity name", examples, element, messages);
    }

    public static bool IsUiElementName(string name) {
      return Is(Style.UpperCamelCase, name);
    }

    public static bool ValidateUiElementName(string uiElementName, IParseElement element, MessageBucket messages) {
      string examples = "'DropDown', 'TextArea'";
      return Validate(Style.UpperCamelCase, uiElementName, "UI Element name", examples, element, messages);
    }

    public static bool IsUiAtomicAttributeName(string name) {
      return Is(Style.LowerCamelCase, name);
    }

    public static bool ValidateUiAtomicAttributeName(string attrName, IParseElement element, MessageBucket messages) {
      string examples = "'visible', 'borderColor'";
      return Validate(Style.LowerCamelCase, attrName, "UI Atomic Attribute Name name", examples, element, messages);
    }

    public static bool IsUiComplexAttributeName(string name) {
      return Is(Style.UpperCamelCase, name);
    }

    public static bool ValidateUiComplexAttributeName(string attrName, IParseElement element, MessageBucket messages) {
      string examples = "'Columns', 'HeaderRows'";
      return Validate(Style.UpperCamelCase, attrName, "UI Complex Attribute name", examples, element, messages);
    }

    public static bool IsMemberName(string name) {
      return Is(Style.SnakeCaseOrLowerCamelCase, name);
    }

    public static bool ValidateAttributeName(string attributeName, IParseElement element, MessageBucket messages) {
      string examples = "'age', 'firstName', 'first_name'";
      return Validate(Style.SnakeCaseOrLowerCamelCase, attributeName, "Attribute name", examples, element, messages);
    }

    public static bool ValidateAssociationName(string associationName, IParseElement element, MessageBucket messages) {
      string examples = "'sender', 'purchaseOrders', 'purchase_orders'";
      return Validate(Style.SnakeCaseOrLowerCamelCase, associationName, "Association name", examples, element, messages);
    }

    public static bool ValidateEnumName(string enumName, IParseElement element, MessageBucket messages) {
      string examples = "'Gender', 'CalendarMonths', 'EnrollmentState'";
      return Validate(Style.UpperCamelCase, enumName, "Enum name", examples, element, messages);
    }

    public static bool ValidateEnumValue(string enumValue, IParseElement element, MessageBucket messages) {
      string examples = "'male', 'awaitingApproval, ASAP'";
      return Validate(Style.LowerCamelCaseOrAllCaps, enumValue, "Enum value", examples, element, messages);
    }

    public static bool ValidateFunctionName(string functionName, IParseElement element, MessageBucket messages) {
      string examples = "'ToHumandReadable'";
      return Validate(Style.UpperCamelCase, functionName, "Function name", examples, element, messages);
    }

    public static bool ValidateFunctionArgumentName(string argumentName, IParseElement element, MessageBucket messages) {
      string examples = "'book', 'dateTime'";
      return Validate(Style.LowerCamelCase, argumentName, "Function Argument name", examples, element, messages);
    }

    private static bool Is(Style style, string text) {
      return Validate(style, text, null, null, null, null);
    }

    private static bool Validate(Style style, string text, string type, string examples, IParseElement element, MessageBucket messages) {
      Regex[] regexes;
      string errorMessage;

      switch (style) {
        case Style.UpperCamelCase:
          regexes = new Regex[] { UPPER_CASE_CAMEL_REGEX };
          errorMessage = string.Format("Must be upper-cased CamelCase: e.g. {0}. Numbers are also allowed.", examples);
          break;
        case Style.LowerCamelCase:
          regexes = new Regex[] { LOWER_CASE_CAMEL_REGEX };
          errorMessage = string.Format("Must be lower-cased camelCase: e.g. {0}. Numbers are also allowed.", examples);
          break;
        case Style.LowerCamelCaseOrAllCaps:
          regexes = new Regex[] { LOWER_CASE_CAMEL_REGEX, ALL_CAPS };
          errorMessage = string.Format("Must be lower-cased camelCase or ALL_CAPS: e.g. {0}. Numbers are also allowed.", examples);
          break;
        case Style.SnakeCaseOrLowerCamelCase:
          regexes = new Regex[] { LOWER_CASE_CAMEL_REGEX, SNAKE_CASE };
          errorMessage = string.Format("Must be lower-cased camelCase or snake_case: e.g. {0}. Numbers are also allowed.", examples);
          break;
        default:
          throw new Exception("Unknown style: " + style);
      }

      foreach (Regex regex in regexes)
        if (MatchesRegex(regex, text))
          return true;

      if (messages != null)
        messages.AddError(element,
          string.Format("Invalid {0}: '{1}'. {2}", type, text, errorMessage));

      return false;
    }

    private static bool MatchesRegex(Regex regex, string text) {
      return regex.IsMatch(text);
    }
  }
}

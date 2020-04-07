using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using x10.parsing;

namespace x10.model {
  public static class ModelValidationUtils {

    private enum Style {
      UpperCamelCase,
      LowerCamelCase,
      LowerCamelCaseOrAllCaps,
    }

    private readonly static Regex UPPER_CASE_CAMEL_REGEX = new Regex("^[A-Z][a-zA-Z0-9]+$");
    private readonly static Regex LOWER_CASE_CAMEL_REGEX = new Regex("^[a-z][a-zA-Z0-9]+$");
    private readonly static Regex ALL_CAPS = new Regex("^[A-Z0-9_]+$");

    public static bool ValidateEntityName(string entityName, IParseElement element, MessageBucket messages) {
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

    public static bool IsMemberName(string name) {
      return Is(Style.LowerCamelCase, name);
    }

    public static bool ValidateAttributeName(string attributeName, IParseElement element, MessageBucket messages) {
      string examples = "'age', 'firstName'";
      return Validate(Style.LowerCamelCase, attributeName, "Attribute name", examples, element, messages);
    }

    public static bool ValidateAssociationName(string associationName, IParseElement element, MessageBucket messages) {
      string examples = "'sender', 'purchaseOrders'";
      return Validate(Style.LowerCamelCase, associationName, "Association name", examples, element, messages);
    }

    public static bool ValidateEnumName(string enumName, IParseElement element, MessageBucket messages) {
      string examples = "'Gender', 'CalendarMonths', 'EnrollmentState'";
      return Validate(Style.UpperCamelCase, enumName, "Enum name", examples, element, messages);
    }

    public static bool ValidateEnumValue(string enumValue, IParseElement element, MessageBucket messages) {
      string examples = "'male', 'awaitingApproval, ASAP'";
      return Validate(Style.LowerCamelCaseOrAllCaps, enumValue, "Enum value", examples, element, messages);
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

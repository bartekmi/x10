using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using x10.parsing;

namespace x10.model {
  public static class ModelValidationUtils {

    private enum Style {
      UpperCamelCase,
      LowerCamelCase,
    }

    private readonly static Regex UPPER_CASE_CAMEL_REGEX = new Regex("^[A-Z][a-zA-Z0-9]+$");
    private readonly static Regex LOWER_CASE_CAMEL_REGEX = new Regex("^[a-z][a-zA-Z0-9]+$");

    public static bool ValidateEntityName(string entityName, TreeElement element, MessageBucket messages) {
      string examples = "'User', 'PurchaseOrder'";
      return Validate(Style.UpperCamelCase, entityName, "Entity name", examples, element, messages);
    }

    public static bool ValidateAttributeName(string attributeName, TreeElement element, MessageBucket messages) {
      string examples = "'age', 'firstName'";
      return Validate(Style.LowerCamelCase, attributeName, "Attribute name", examples, element, messages);
    }

    public static bool ValidateAssociationName(string associationName, TreeElement element, MessageBucket messages) {
      string examples = "'sender', 'purchaseOrders'";
      return Validate(Style.LowerCamelCase, associationName, "Association name", examples, element, messages);
    }

    public static bool ValidateEnumName(string enumName, TreeElement element, MessageBucket messages) {
      string examples = "'Gender', 'CalendarMonths', 'EnrollmentState'";
      return Validate(Style.UpperCamelCase, enumName, "Enum name", examples, element, messages);
    }

    public static bool ValidateEnumValue(string enumValue, TreeElement element, MessageBucket messages) {
      string examples = "'male', 'awaitingApproval'";
      return Validate(Style.LowerCamelCase, enumValue, "Enum value", examples, element, messages);
    }

    private static bool Validate(Style style, string text, string type, string examples, TreeElement element, MessageBucket messages) {

      Regex regex = null;
      string errorMessage = null;

      switch (style) {
        case Style.UpperCamelCase:
          regex = UPPER_CASE_CAMEL_REGEX;
          errorMessage = string.Format("Must be upper-cased camel-case: e.g. {0}. Numbers are also allowed.", examples);
          break;
        case Style.LowerCamelCase:
          regex = LOWER_CASE_CAMEL_REGEX;
          errorMessage = string.Format("Must be lower-case camel values: e.g. {0}. Numbers are also allowed.", examples);
          break;
        default:
          throw new Exception("Unknown style: " + style);
      }

      if (MatchesRegex(regex, text))
        return true;

      messages.AddError(element,
        string.Format("Invalid {0}: '{1}'. {2}", type, text, errorMessage));

      return false;
    }

    private static bool MatchesRegex(Regex regex, string text) {
      return regex.IsMatch(text);
    }
  }
}

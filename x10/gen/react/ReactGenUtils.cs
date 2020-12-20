using System;

using x10.model.metadata;
using x10.utils;

namespace x10.gen.react {
  internal static class ReactGenUtils {

    internal static string TypedLiteralToString(object literal, DataTypeEnum asEnum) {
      if (literal == null)
        return "null";

      if (asEnum != null)
        return string.Format("'{0}'", NameUtils.CamelCaseToSnakeCaseAllCaps(literal?.ToString()));

      if (literal is string)
        return string.Format("'{0}'", literal);
      else if (literal is bool)
        return literal.ToString().ToLower();
      else
        return literal.ToString();
    }

    internal static string EnumToName(DataTypeEnum enumType) {
      return enumType.Name + "Enum";
    }
  }
}

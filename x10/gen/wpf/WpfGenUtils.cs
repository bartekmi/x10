using System;
using System.Collections.Generic;
using System.Text;
using x10.model.definition;
using x10.model.metadata;
using x10.utils;

namespace x10.gen.wpf {
  internal static class WpfGenUtils {

    internal const string MODEL_PROPERTY = "Model";
    internal const string MODEL_PROPERTY_PREFIX = MODEL_PROPERTY + ".";

    internal static string TypedLiteralToString(object literal, DataTypeEnum asEnum) {

      if (asEnum != null)
        return string.Format("{0}.{1}", EnumToName(asEnum), NameUtils.Capitalize(literal?.ToString()));

      if (literal is string)
        return string.Format("\"{0}\"", literal);
      else if (literal is bool)
        return literal.ToString().ToLower();
      else
        return literal.ToString();
    }

    internal static string MemberToName(Member member) {
      string name = NameUtils.Capitalize(member.Name);

      // In C#, a class member name may not be the same as enclosing class
      if (name == member.Owner.Name)
        name = "The" + name;

      return name;
    }

    internal static string EnumToName(DataTypeEnum enumType) {
      return enumType.Name + "Enum";
    }
  }
}

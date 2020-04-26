using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace x10.utils {
  public static class NameUtils {
    public static string SnakeCaseToCamelCase(string snake_case) {
      StringBuilder builder = new StringBuilder();
      bool capitalize = true;

      foreach (char c in snake_case) {
        if (c == '_') {
          capitalize = true;
          continue;
        }

        builder.Append(capitalize ? char.ToUpper(c) : c);
        capitalize = false;
      }

      return builder.ToString();
    }

    public static string CamelCaseToSnakeCase(string camelOrPascalCase) {
      StringBuilder builder = new StringBuilder();
      bool isInCapital = true;

      foreach (char c in camelOrPascalCase) {
        if (char.IsUpper(c)) {
          if (!isInCapital)
            builder.Append('_');
          isInCapital = true;
        } else if (char.IsLower(c))
          isInCapital = false;

        builder.Append(char.ToLower(c));
      }

      return builder.ToString();
    }

    public static string CamelCaseToHumanReadable(string name, bool capitalizeFirstLetter = false) {
      StringBuilder builder = new StringBuilder();

      foreach (char c in name) {
        if (char.IsUpper(c) && builder.Length > 0)
          builder.Append(' ');
        builder.Append(c);
      }

      string text = builder.ToString();

      if (capitalizeFirstLetter && text.Length > 0)
        text = char.ToUpper(text[0]) + text.Substring(1, text.Length - 1);

      return text;
    }
  }
}

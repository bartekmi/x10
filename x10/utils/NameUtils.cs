﻿using System;
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

    public static string CamelCaseToSnakeCaseAllCaps(string camelOrPascalCase) {
      string snakeCase = CamelCaseToSnakeCase(camelOrPascalCase);
      return snakeCase.ToUpper();
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

    public static string CamelCaseToHumanReadable(string name, bool capitalizeFirstLetter = true) {
      if (name == null)
        return null;

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

    public static string Capitalize(string text) {
      bool capitalize = true;
      StringBuilder builder = new StringBuilder();

      foreach (char c in text) {
        builder.Append(capitalize ? char.ToUpper(c) : c);

        if (char.IsWhiteSpace(c))
          capitalize = true;
        else
          capitalize = false;
      }

      return builder.ToString();
    }

    internal static string CapitalizeFirstLetter(string text) {
      if (string.IsNullOrWhiteSpace(text))
        return text;
      return char.ToUpper(text[0]) + text.Substring(1);
    }

    internal static string UncapitalizeFirstLetter(string text) {
      if (string.IsNullOrWhiteSpace(text))
        return text;
      return char.ToLower(text[0]) + text.Substring(1);
    }

    public static bool IsQuoted(string text) {
      text = text.Trim();
      return text.StartsWith("'") && text.EndsWith("'");
    }

    public static string StripQuotes(string quotedText) {
      quotedText = quotedText.Trim();
      return quotedText.Trim('\'').Trim('"');
    }

    public static string Pluralize(string singular) {
      if (singular.EndsWith("s"))
        return singular + "es";
      if (singular.EndsWith("y"))
        return singular.Substring(0, singular.Length - 1) + "ies";
      return singular + "s";
    }
  }
}

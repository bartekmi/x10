using System;
using System.Collections.Generic;
using System.Text;

namespace x10.gen.wpf {
  internal static class WpfGenUtils {
    internal static string TypedLiteralToString(object literal) {
      if (literal is String)
        return String.Format("\"{0}\"", literal);
      else if (literal is Boolean)
        return literal.ToString().ToLower();
      else
        return literal.ToString();
    }
  }
}

﻿using System;
using System.Collections.Generic;
using System.Text;
using x10.model.metadata;
using x10.utils;

namespace x10.gen.wpf {
  internal static class WpfGenUtils {
    internal static string TypedLiteralToString(object literal, DataTypeEnum asEnum) {

      if (asEnum != null)
        return string.Format("{0}.{1}", asEnum.Name, NameUtils.Capitalize(literal?.ToString()));

      if (literal is string)
        return string.Format("\"{0}\"", literal);
      else if (literal is bool)
        return literal.ToString().ToLower();
      else
        return literal.ToString();
    }
  }
}
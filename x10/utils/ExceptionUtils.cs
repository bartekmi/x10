using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace x10.utils {
  public static class ExceptionUtils {
    public static string GetMessageRecursively(Exception exception) {
      StringBuilder builder = new StringBuilder();

      while (exception != null) {
        if (builder.Length > 0)
          builder.Append(": ");
        builder.Append(exception.Message);
        exception = exception.InnerException;
      }

      return builder.ToString();
    }
  }
}

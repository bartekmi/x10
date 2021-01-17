using System;
using System.Collections.Generic;
using System.Linq;

internal static class JS {

  // Qs stands for Quote String, but is shortened in anticipation of heavy use
  internal static string Qs(string text) {
    return string.Format("'{0}'", text);
  }

  internal static string ToArray(IEnumerable<string> data) {
    if (data == null)
      return null;

    return string.Format("[{0}]", string.Join(", ", data.Select(x => Qs(x))));
  }
}
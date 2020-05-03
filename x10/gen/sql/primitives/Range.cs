using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace x10.gen.sql.primitives {
  internal class Range {
    internal int From;
    internal int To;

    internal int GetRandom(Random random) {
      return random.Next(From, To + 1);
    }

    internal static Range Parse(string text) {
      string[] pieces = text.Trim().Split("..");
      if (pieces.Length != 2)
        return null;

      if (!int.TryParse(pieces[0], out int from) ||
          !int.TryParse(pieces[1], out int to))
        return null;

      if (to <= from)
        return null;

      return new Range() {
        From = from,
        To = to,
      };
    }
  }
}

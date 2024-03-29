﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace x10.gen.sql.primitives {
  internal class SqlRange {
    internal int From;
    internal int To;

    // Derived
    internal bool IsZero { get { return From == 0 && To == 0; } }

    internal int GetRandom(Random random) {
      return random.Next(From, To + 1);
    }

    internal static SqlRange Parse(string text) {
      if (int.TryParse(text, out int singleValue)) {
        return new SqlRange() {
          From = singleValue,
          To = singleValue,
        };
      }

      string[] pieces = text.Trim().Split("..");
      if (pieces.Length != 2)
        return null;

      if (!int.TryParse(pieces[0], out int from) ||
          !int.TryParse(pieces[1], out int to))
        return null;

      if (to <= from)
        return null;

      return new SqlRange() {
        From = from,
        To = to,
      };
    }
  }
}

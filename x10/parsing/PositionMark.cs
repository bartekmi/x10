using System;
using System.Collections.Generic;
using System.Text;

namespace x10.parsing {
  public class PositionMark {
    public int LineNumber { get; set; }
    public int CharacterPosition { get; set; }
    public int Index { get; set; }

    public override string ToString() {
      return string.Format("(Line = {0}, Char = {1}, Index = {2})", LineNumber, CharacterPosition, Index);
    }

    internal PositionMark AdvanceBy(int numberOfCharacters) {
      return new PositionMark() {
        LineNumber = LineNumber,
        CharacterPosition = CharacterPosition + numberOfCharacters,
        Index = Index + numberOfCharacters,
      };
    }
  }
}

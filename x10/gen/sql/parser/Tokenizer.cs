using System;

namespace x10.gen.sql.parser {
  internal class Tokenizer {
    private int _index = 0;
    private readonly string _text;

    // Derived
    internal bool HasMore { get { return _index < _text.Length; } }

    internal Tokenizer(string text) {
      _text = text;
    }

    internal char Next() {
      if (!HasMore)
        throw new Exception("Unexpected end of string");
      return _text[_index++];
    }

    internal char? Peek() {
      if (HasMore)
        return _text[_index];
      return null;
    }

    internal void Expect(char c) {
      EatWhitespace();

      char next = Next();
      if (next != c)
        throw new Exception(string.Format("Expected {0} but got {1} at index {2}", c, next, _index - 1));
    }

    internal void EatWhitespace() {
      while (Peek() == ' ' || Peek() == '\t')
        Next();
    }

    #region Mark
    private int _mark;
    internal void SetMark() {
      _mark = _index;
    }

    internal void ResetToMark() {
      _index = _mark;
    }
    #endregion
  }
}
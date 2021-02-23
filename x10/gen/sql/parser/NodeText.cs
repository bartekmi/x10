using System;
using System.Text;

namespace x10.gen.sql.parser {
  class NodeText : Node {
    internal Delimiter Delimiter;
    private readonly StringBuilder _builder = new StringBuilder();

    // Derived
    private string _trimmedText;
    internal String Text { get { return _trimmedText ?? _builder.ToString(); } }
    internal DelimiterType? Type { get { return Delimiter?.Type; } }
    internal bool Empty { get { return _builder.Length == 0; } }

    internal void Add(char c) {
      _builder.Append(c);
    }

    internal void TrimStart() {
      _trimmedText = Text.TrimStart();
    }

    internal void TrimEnd() {
      _trimmedText = Text.TrimEnd();
    }

    internal override void Print(StringBuilder builder) {
      if (Delimiter == null)
        builder.Append(Text);
      else
        builder.Append(string.Format("{0}{1}{2}",
          Delimiter.Open, Text, Delimiter.Close));
    }
  }
}
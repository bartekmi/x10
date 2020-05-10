using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using x10.parsing;
using YamlDotNet.Core.Tokens;

namespace x10.gen.sql.primitives {
  // Parses strings like:
  // Some text ( 50% = <adjective> | 50% = <noun> *LLDD.DD* (80% = Hello | 20% = World)) more text

  // (The intention is to generate data like...)
  // Some text red more text
  // Some text horse AF52.69 Hello more text

  // The resulting parse tree should be:

  // (Concat)
  //    (Text) "Some text "
  //    (Probabilities)
  //      (Concat @ 50%)
  //        (Text @ <>) "adjective"
  //      (Concat @ 50%)
  //        (Text @ <>) "noun"
  //        (Text) " "
  //        (Text @ **) "DD.DD"
  //        (Text) " "
  //        (Probabilities): 
  //          (Concat @ 80%)
  //            (Text) "Hello"
  //          (Concat @ 20%)
  //            (Text) "World"
  //   (Text) " more text"

  #region Helper Classes

  enum DelimiterType {
    DictionaryReplace,
    CharacterReplace,
  }

  // Defines a special delimiter - e.g. (...)
  class Delimiter {
    internal DelimiterType Type;
    internal char Open;
    internal char Close;

    internal Delimiter(DelimiterType type, char open, char close) {
      Type = type;
      Open = open;
      Close = close;
    }
  }

  abstract class Node : IWithProbability {
    internal abstract void Print(StringBuilder builder);
    public double Probability { get; internal set; }
    internal List<Node> Children = new List<Node>();

    // Derived
    internal string OnlyChildText {
      get {
        return Children.OfType<NodeText>().Single().Text;
      }
    }
  }

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

  class NodeConcat : Node {
    internal override void Print(StringBuilder builder) {
      foreach (Node child in Children)
        child.Print(builder);
    }

    // Trim whitespace at both ends
    internal void Trim() {
      if (Children.First() is NodeText first)
        first.TrimStart();
      if (Children.Last() is NodeText last)
        last.TrimEnd();
    }
  }

  class NodeProbabilities : Node {
    internal override void Print(StringBuilder builder) {
      builder.Append("( ");
      foreach (Node child in Children) {
        builder.Append(child.Probability * 100);
        builder.Append("% = ");
        child.Print(builder);
        if (child != Children.Last())
          builder.Append(" | ");
      }
      builder.Append(" )");
    }
  }
  #endregion

  #region Tokenizer
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
  }
  #endregion

  #region Parser
  internal class DataGenLanguageParser {
    private static readonly Delimiter[] DELIMITERS = new Delimiter[] {
      new Delimiter(DelimiterType.DictionaryReplace, '<', '>'),
      new Delimiter(DelimiterType.CharacterReplace, '~', '~'),
    };

    private readonly MessageBucket _messages;

    internal DataGenLanguageParser(MessageBucket messages) {
      _messages = messages;
    }

    internal NodeConcat Parse(string text) {
      Tokenizer tokenizer = new Tokenizer(text);
      return Parse(tokenizer);
    }

    private NodeConcat Parse(Tokenizer tokenizer) {
      NodeConcat concat = new NodeConcat();

      NodeText text = new NodeText(); // Current running text accumulator

      while (true) {
        char? next = tokenizer.Peek();
        if (next == null || next.Value == '|' || next.Value == ')')
          break;

        if (next.Value == '(') {    // NodeProbabilities
          text = RecordTextNode(concat, text);
          concat.Children.Add(ParseProbabilities(tokenizer));
        } else {
          char c = tokenizer.Next();
          if (text.Delimiter != null && c == text.Delimiter.Close) {
            text = RecordTextNode(concat, text);
          } else if (DELIMITERS.Select(x => x.Open).Any(x => x == c)) {
            text = RecordTextNode(concat, text);
            text.Delimiter = DELIMITERS.Single(x => x.Open == c);
          } else
            text.Add(c);
        }
      }

      RecordTextNode(concat, text);

      concat.Trim();

      return concat;
    }

    private NodeText RecordTextNode(NodeConcat concat, NodeText text) {
      if (text.Empty)
        return text;

      concat.Children.Add(text);
      return new NodeText();
    }

    private Node ParseProbabilities(Tokenizer tokenizer) {
      NodeProbabilities probabilities = new NodeProbabilities();
      tokenizer.Expect('(');

      while (true) {
        double probability = ParseProbability(tokenizer);
        tokenizer.Expect('=');

        Node child = Parse(tokenizer);
        child.Probability = probability;
        probabilities.Children.Add(child);

        char terminator = tokenizer.Next();
        if (terminator == ')')
          break;
        if (terminator != '|')
          throw new Exception("Unexpected terminator " + terminator);
      }

      return probabilities;
    }

    private double ParseProbability(Tokenizer tokenizer) {
      int percentage = ParseInt(tokenizer);
      if (percentage < 0 || percentage > 100)
        throw new Exception("Percentage must be between 0 and 100");
      tokenizer.Expect('%');
      return percentage / 100.0;
    }

    private int ParseInt(Tokenizer tokenizer) {
      tokenizer.EatWhitespace();
      int result = 0;
      bool encounteredDigit = false;

      do {
        char? c = tokenizer.Peek();
        if (c == null || !char.IsDigit(c.Value))
          break;

        tokenizer.Next();
        result *= 10;
        result += c.Value - '0';
        encounteredDigit = true;
      } while (true);

      if (!encounteredDigit)
        throw new Exception("Could not parse integer");

      return result;
    }
  }
  #endregion
}

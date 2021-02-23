using System;
using System.Linq;

using x10.parsing;

namespace x10.gen.sql.parser {
  // Parses strings like:
  // Some text ( 50% => <adjective> | 50% => <noun> ~LLDD.DD~ (80% => Hello | 20% => World)) more text

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
  //        (Text @ ~~) "DD.DD"
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
        tokenizer.SetMark();
        double? probability = MaybeParseProbability(tokenizer);
        if (probability == null)
          tokenizer.ResetToMark();
        else {
          tokenizer.Expect('=');
          tokenizer.Expect('>');
        }

        Node child = Parse(tokenizer);
        child.Probability = probability ?? Double.MinValue;
        probabilities.Children.Add(child);

        char terminator = tokenizer.Next();
        if (terminator == ')')
          break;
        if (terminator != '|')
          throw new Exception("Unexpected terminator " + terminator);
      }

      // Any left over probability not allocated gets distributed to children
      // which did not specify a probability
      double totalAllocatedProbability = probabilities.Children
        .Select(x => x.Probability).Where(x => x != Double.MinValue).Sum();
      int noProbabilityChildCount = probabilities.Children
        .Select(x => x.Probability).Where(x => x == Double.MinValue).Count();

      foreach (Node child in probabilities.Children.Where(x => x.Probability == Double.MinValue))
        child.Probability = (1.0 - totalAllocatedProbability) / noProbabilityChildCount;

      return probabilities;
    }

    private double? MaybeParseProbability(Tokenizer tokenizer) {
      int? percentage = MaybeParseInt(tokenizer);
      if (percentage == null)
        return null;

      if (percentage < 0 || percentage > 100)
        throw new Exception("Percentage must be between 0 and 100");
      tokenizer.Expect('%');
      return percentage / 100.0;
    }

    private int? MaybeParseInt(Tokenizer tokenizer) {
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
        return null;

      return result;
    }
  }
  #endregion
}

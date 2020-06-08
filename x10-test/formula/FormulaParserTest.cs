using System;
using System.Linq;
using System.Collections.Generic;

using Xunit;
using Xunit.Abstractions;
using x10.parsing;
using x10.formula;

namespace x10.utils {
  public class FormulaParserTest {

    [Fact]
    public void ParseWithSyntaxErrors() {
      Test("a + b)", "Unexpected token ')'", 0, 5);
    }

    private void Test(string formula, string expectedError, int startCharPos, int endCharPos) {
      MessageBucket errors = new MessageBucket();
      IParseElement element = new BasicParseElement() {
        Start = new PositionMark() {
          LineNumber = 10,
          CharacterPosition = 100,
          Index = 1000,
        },
      };

      FormulaParser.Parse(errors, element, formula, null);
      Assert.Equal(1, errors.Count);

      CompileMessage message = errors.Messages.Single();
      IParseElement newElement = message.ParseElement;

      Assert.Equal(expectedError, message.Message);
      Assert.Same(newElement.FileInfo, element.FileInfo);
      Assert.Equal(10, newElement.Start.LineNumber);
      Assert.Equal(100 + startCharPos, newElement.Start.CharacterPosition);
      Assert.Equal(100 + endCharPos, newElement.End.CharacterPosition);
    }
  }
}
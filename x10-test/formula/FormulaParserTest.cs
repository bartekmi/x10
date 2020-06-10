using System;
using System.Linq;
using System.Collections.Generic;

using Xunit;
using Xunit.Abstractions;
using x10.parsing;
using x10.formula;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace x10.utils {
  public class FormulaParserTest {

    [Fact]
    public void ParseSuccessful() {
      TestExpectedSuccess("1 + 2.7");
      TestExpectedSuccess("\"Hello\" + \" World\"");
      TestExpectedSuccess("(1 + a) / b");
      TestExpectedSuccess("MyFunc(a, b, 7)");
      TestExpectedSuccess("a * b + c * d / e");
      TestExpectedSuccess("a < 7");
      TestExpectedSuccess("myBool && MyFunc(x)");
      TestExpectedSuccess("a.b");
    }

    [Fact]
    public void ParseWithSyntaxErrors() {
      TestExpectedError("a + b)", "Unexpected token ')'", 0, 5);
    }

    private void TestExpectedSuccess(string formula) {
      MessageBucket errors = new MessageBucket();
      IParseElement element = new XmlElement("Dummy") { Start = new PositionMark() };
      ExpBase expression = FormulaParser.Parse(errors, element, formula, null);

      Assert.Equal(0, errors.Count);
      Assert.False(expression is ExpUnknown);
    }

    private void TestExpectedError(string formula, string expectedError, int startCharPos, int endCharPos) {
      MessageBucket errors = new MessageBucket();
      IParseElement element = new XmlElement("Dummy") {
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
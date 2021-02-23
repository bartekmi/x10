using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

using Xunit;
using Xunit.Abstractions;

using x10.parsing;
using x10.model;
using x10.ui.libraries;
using x10.ui.metadata;
using x10.compiler;
using x10.model.definition;
using System.Text;

namespace x10.gen.sql.parser {
  public class DataGenLanguageParserTest {

    [Fact]
    public void ParseText() {
      RunTest("Start text");
    }

    [Fact]
    public void ParseTextWithDelimiters() {
      RunTest("*LL.DD*");
    }

    [Fact]
    public void ParseTextWithEscapedChars() {
      RunTest("\\(\\)", "()");
    }

    [Fact]
    public void ParseConcatWithDelimiters() {
      RunTest("Start *LL.DD* Middle <noun> End");
    }

    [Fact]
    public void ParseProbabilities() {
      RunTest("( 10% => Hello | 90% => World )");
    }

    [Fact]
    public void ParseProbabilitiesWithDefaults() {
      RunTest("( 20% => A | B | C )",
        "( 20% => A | 40% => B | 40% => C )");
    }

    [Fact]
    public void ParseConcat() {
      RunTest("Start ( 10% => Hello | 90% => World ) Finish");
    }

    [Fact]
    public void ParseConcatAndRecursiveProbabilities() {
      RunTest("Start text ( 50% => First | 50% => Second ( 80% => Hello | 20% => World ) ) End Text");
    }

    private void RunTest(string input, string output = null) {
      MessageBucket messages = new MessageBucket();
      DataGenLanguageParser parser = new DataGenLanguageParser(messages);
      Node node = parser.Parse(input);
      StringBuilder builder = new StringBuilder();
      node.Print(builder);

      Assert.Empty(messages.Messages);
      Assert.Equal(output ?? input, builder.ToString());
    }
  }
}

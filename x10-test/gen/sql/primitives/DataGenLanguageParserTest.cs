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

namespace x10.gen.sql.primitives {
  public class DataGenLanguageParserTest {

    private readonly ITestOutputHelper _output;

    public DataGenLanguageParserTest(ITestOutputHelper output) {
      _output = output;
    }

    [Fact]
    public void ParseText() {
      RunTest("Start text");
    }

    [Fact]
    public void ParseTextWithDelimiters() {
      RunTest("*LL.DD*");
    }

    [Fact]
    public void ParseConcatWithDelimiters() {
      RunTest("Start *LL.DD* Middle <noun> End");
    }

    [Fact]
    public void ParseProbabilities() {
      RunTest("( 10% = Hello | 90% = World )");
    }

    [Fact]
    public void ParseConcat() {
      RunTest("Start ( 10% = Hello | 90% = World ) Finish");
    }

    [Fact]
    public void ParseConcatAndRecursiveProbabilities() {
      RunTest("Start text ( 50% = First | 50% = Second ( 80% = Hello | 20% = World ) ) End Text");
    }

    private void RunTest(string input, string output = null) {
      Node node = DataGenLanguageParser.Parse(input);
      StringBuilder builder = new StringBuilder();
      node.Print(builder);

      Assert.Equal(output ?? input, builder.ToString());
    }
  }
}

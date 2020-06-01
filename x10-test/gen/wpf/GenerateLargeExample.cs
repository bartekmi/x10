using System.Linq;
using System.Collections.Generic;

using Xunit;
using Xunit.Abstractions;

using x10.parsing;
using x10.compiler;
using x10.model;

namespace x10.gen.wpf {
  public class GenerateLargeExample {

    private readonly ITestOutputHelper _output;
    private readonly MessageBucket _messages = new MessageBucket();

    public GenerateLargeExample(ITestOutputHelper output) {
      _output = output;
    }

    [Fact]
    public void Generate() {
      string sourceDir = "../../../../x10/examples/flexport";
      LargeDemoTest.CompileEverything(_output, _messages, sourceDir, out AllEntities allEntities, out AllEnums allEnums, out AllUiDefinitions allUiDefinitions);

      string targetDir = "../../../../wpf_generated/__generated__";
      WpfCodeGenerator generator = new WpfCodeGenerator(targetDir, "wpf_generated", allEntities, allEnums, allUiDefinitions);
      generator.Generate();
    }
  }
}
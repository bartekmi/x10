using System.Linq;
using System.Collections.Generic;

using Xunit;
using Xunit.Abstractions;

using x10.parsing;
using x10.compiler;
using x10.model;
using x10.ui.platform;
using x10.ui.libraries;

namespace x10.gen.react {
  public class GenerateSmallExample {

    private readonly ITestOutputHelper _output;
    private readonly MessageBucket _messages = new MessageBucket();

    public GenerateSmallExample(ITestOutputHelper output) {
      _output = output;
    }

    [Fact]
    public void Generate() {
      string sourceDir = "../../../../x10/examples/small";
      LargeDemoTest.CompileEverything(_output, _messages, sourceDir,
        out AllEntities allEntities,
        out AllEnums allEnums,
        out AllFunctions allFuncs,
        out AllUiDefinitions allUiDefinitions);

      LatitudeLibrary.Singleton().HydrateAndValidate(_messages);
      PlatformLibrary[] libraries = new PlatformLibrary[] {
        LatitudeLibrary.Singleton(),
      };

      TestUtils.DumpMessages(_messages, _output);
      Assert.Empty(_messages.Errors);

      string targetDir = "../../../../react_generated_small/__generated__";
      ReactCodeGenerator generator = new ReactCodeGenerator();

      _messages.Clear();
      generator.Generate(
        _messages,
        targetDir,
        allEntities,
        allEnums,
        allUiDefinitions,
        libraries);

      TestUtils.DumpMessages(_messages, _output);
    }
  }
}
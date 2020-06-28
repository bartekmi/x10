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

namespace x10.compiler {
  public class LargeDemoTest {

    private readonly ITestOutputHelper _output;
    private readonly MessageBucket _messages;

    public LargeDemoTest(ITestOutputHelper output) {
      _output = output;
      _messages = new MessageBucket();
    }

    [Fact]
    public void CompileEntityFiles() {
      string rootDir = "../../../../x10/examples/flexport";
      EntitiesAndEnumsCompiler compiler = new EntitiesAndEnumsCompiler(_messages, new AllEnums(_messages), new AllFunctions(_messages));
      compiler.Compile(rootDir);

      TestUtils.DumpMessages(_messages, _output, CompileMessageSeverity.Error);

      int errorCount = _messages.FilteredMessages(CompileMessageSeverity.Error).Count();
      Assert.Equal(0, errorCount);
    }

    [Fact]
    public void CompileEverythingTest() {
      string rootDir = "../../../../x10/examples/flexport";
      CompileEverything(_output, _messages, rootDir, out AllEntities allEntities, out AllEnums allEnums, out AllFunctions allFuncs, out AllUiDefinitions allUiDefinitions);

      int errorCount = _messages.FilteredMessages(CompileMessageSeverity.Error).Count();
      Assert.Equal(0, errorCount);
    }

    internal static void CompileEverything(ITestOutputHelper output, MessageBucket messages, string rootDir,
      out AllEntities allEntities, out AllEnums allEnums, out AllFunctions allFunctions, out AllUiDefinitions allUiDefinitions) {

      IEnumerable<UiLibrary> libraries = new UiLibrary[] {
        BaseLibrary.Singleton(),
        IconLibrary.Singleton(),
        LargeDemoLibrary.Singleton(),
      };

      foreach (UiLibrary library in libraries)
        if (!library.HydrateAndValidate(messages)) {
          TestUtils.DumpMessages(messages, output);
          Assert.Empty(messages.Messages);
        }

      TopLevelCompiler compiler = new TopLevelCompiler(messages, libraries);
      compiler.Compile(rootDir, out allEntities, out allEnums, out allFunctions, out allUiDefinitions);

      TestUtils.DumpMessages(messages, output, CompileMessageSeverity.Error);
    }
  }
}

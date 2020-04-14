using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

using Xunit;
using Xunit.Abstractions;

using x10.parsing;
using x10.model;
using x10.ui.libraries;

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
      EntitiesAndEnumsCompiler compiler = new EntitiesAndEnumsCompiler(_messages, new AllEnums(_messages));
      compiler.Compile(rootDir);

      TestUtils.DumpMessages(_messages, _output, CompileMessageSeverity.Error);

      int errorCount = _messages.FilteredMessages(CompileMessageSeverity.Error).Count();
      Assert.Equal(0, errorCount);
    }

    [Fact]
    public void CompileEverything() {
      string rootDir = "../../../../x10/examples/flexport";
      TopLevelCompiler compiler = new TopLevelCompiler(_messages, BaseLibrary.Singleton);
      compiler.Compile(rootDir, out AllEntities allEntities, out AllEnums allEnums, out AllUiDefinitions allUiDefinitions);

      TestUtils.DumpMessages(_messages, _output, CompileMessageSeverity.Error);

      int errorCount = _messages.FilteredMessages(CompileMessageSeverity.Error).Count();
      Assert.Equal(0, errorCount);
    }
  }
}

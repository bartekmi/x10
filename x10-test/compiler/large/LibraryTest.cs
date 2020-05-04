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
using x10.model.metadata;

namespace x10.compiler {
  public class LibraryTest {

    private readonly ITestOutputHelper _output;
    private readonly MessageBucket _messages;

    public LibraryTest(ITestOutputHelper output) {
      _output = output;
      _messages = new MessageBucket();
      DataTypes.Singleton.AddDataType(new DataType() {
        Name = "Year",
        ParseFunction = (s) => new ParseResult(int.Parse(s)),
        Examples = "1995, 2020",
      });
    }

    [Fact]
    public void CompileLibraryEntityFiles() {
      string rootDir = "../../../../x10/examples/library";
      EntitiesAndEnumsCompiler compiler = new EntitiesAndEnumsCompiler(_messages, new AllEnums(_messages));
      compiler.Compile(rootDir);

      TestUtils.DumpMessages(_messages, _output, CompileMessageSeverity.Error);

      int errorCount = _messages.FilteredMessages(CompileMessageSeverity.Error).Count();
      Assert.Equal(0, errorCount);
    }
  }
}

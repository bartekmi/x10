﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

using Xunit;
using Xunit.Abstractions;

using x10.model.definition;
using x10.model.metadata;
using x10.parsing;


namespace x10.compiler {
  public class LargeDemoTest {

    private readonly ITestOutputHelper _output;

    public LargeDemoTest(ITestOutputHelper output) {
      _output = output;
    }

    [Fact]
    public void CompileValidFile() {
      string rootDir = "../../../../x10/examples/flexport";
      EntitiesCompiler compiler = new EntitiesCompiler();
      compiler.Compile(rootDir);

      TestUtils.DumpMessages(compiler.Messages, _output);

      Assert.Equal(0, compiler.Messages.Count);
    }
  }
}

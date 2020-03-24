using System;
using System.Linq;
using System.Collections.Generic;

using Xunit;
using Xunit.Abstractions;

using x10.model.definition;
using x10.parsing;

namespace x10.compiler {
  public class EntitiesCompilerTest {

    private readonly ITestOutputHelper _output;

    public EntitiesCompilerTest(ITestOutputHelper output) {
      _output = output;
    }

    [Fact]
    public void CompileValidFile() {
      EntitiesCompiler compiler = new EntitiesCompiler();
      List<Entity> entities = compiler.Compile("../../../compiler/data");

      foreach (CompileMessage message in compiler.Messages.Messages)
        _output.WriteLine(message.ToString());

      Assert.Equal(0, compiler.Messages.Count);
    }
  }
}
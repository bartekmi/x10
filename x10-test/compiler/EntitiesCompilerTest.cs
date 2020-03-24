using System;
using System.Linq;
using System.Collections.Generic;

using Xunit;
using Xunit.Abstractions;

using x10.model.definition;
using x10.parsing;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using System.IO;

namespace x10.compiler {
  public class EntitiesCompilerTest {

    private readonly ITestOutputHelper _output;
    private EntitiesCompiler _compiler = new EntitiesCompiler();

    public EntitiesCompilerTest(ITestOutputHelper output) {
      _output = output;
    }

    [Fact]
    public void CompileValidFile() {
      List<Entity> entities = _compiler.Compile("../../../compiler/data");
      ShowErrors();
      Assert.Equal(0, _compiler.Messages.Count);
    }

    [Fact]
    public void WrongRootNodeType() {
      RunTest(
@"- one
- two
", 
        "The root node of an entity must be a Hash, but was: TreeSequence");
    }

    private void RunTest(string yaml, string expectedErrorMessage) {
      const string TMP_YAML_FILE = "tmp.yaml";
      File.WriteAllText(TMP_YAML_FILE, yaml);
      ParserYaml parser = new ParserYaml();
      TreeNode rootNode = parser.Parse(TMP_YAML_FILE);
      Assert.NotNull(rootNode);

      _compiler.CompileEntity(rootNode);
      ShowErrors();

      CompileMessage message = _compiler.Messages.Messages.Single(x => x.Message == expectedErrorMessage);
    }

    private void ShowErrors() {
      foreach (CompileMessage message in _compiler.Messages.Messages)
        _output.WriteLine(message.ToString());
    }
  }
}
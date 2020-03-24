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
      RunTest(@"
- one
- two
",
        "The root node of an entity must be a Hash, but was: TreeSequence", 2, 1);
    }

    [Fact]
    public void EnumValuesMissing() {
      RunTest(@"
name: Tmp
description: Description...
enums:
  - name: MyEnum
    description: This my awesome enum
",
        "Mandatory enum property 'values' missing", 5, 5);
    }

    [Fact]
    public void ExpectedHashWhenGettingAttributes() {
      RunTest(@"
name: Tmp
description: Description...
attributes:
  - one
",
        "Expected a Hash type node, but was: TreeScalar", 5, 5);
    }

    [Fact]
    public void MissingAttribute() {
      RunTest(@"
name: Tmp
",
        "The attribute 'description' is missing from Entity", 2, 1);
    }

    private void RunTest(string yaml, string expectedErrorMessage, int expectedLine, int expectedChar) {
      const string TMP_YAML_FILE = "Tmp.yaml";
      File.WriteAllText(TMP_YAML_FILE, yaml);
      ParserYaml parser = new ParserYaml();
      TreeNode rootNode = parser.Parse(TMP_YAML_FILE);
      rootNode.SetFileInfo(TMP_YAML_FILE);
      Assert.NotNull(rootNode);

      _compiler.CompileEntity(rootNode);
      ShowErrors();

      CompileMessage message = _compiler.Messages.Messages.SingleOrDefault(x => x.Message == expectedErrorMessage);
      Assert.NotNull(message);

      Assert.Equal(expectedLine, message.TreeElement.Start.LineNumber);
      Assert.Equal(expectedChar, message.TreeElement.Start.CharacterPosition);
    }

    private void ShowErrors() {
      foreach (CompileMessage message in _compiler.Messages.Messages)
        _output.WriteLine(message.ToString());
    }
  }
}
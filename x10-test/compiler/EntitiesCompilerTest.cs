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
    public void CompileWithSetterAndNonSetter() {
      ModelAttributeDefinitions.All.Add(new ModelAttributeDefinition() {
        Name = "customField",
        Description = "This is a custom field with no setter",
        AppliesTo = AppliesTo.Entity,
        DataType = DataTypes.Singleton.String,
      });

      Entity entity = RunTest(@"
name: Tmp
description: My description...
customField: My custom value
");

      Assert.Equal("Tmp", entity.Name);
      Assert.Equal("My description...", entity.Description);
      Assert.Equal("My custom value", AttributeUtils.FindValue(entity, "customField"));
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

    [Fact]
    public void AttributeNotScalar() {
      RunTest(@"
name: Tmp
description: {}
",
        "The attribute 'description' should be simple string of the correct type, but is a TreeHash", 3, 14);
    }

    [Fact]
    public void WrongTypeOfAttribute() {
      RunTest(@"
name: Tmp
description: Description
attributes:
  - name: myBoolean
    description: This is my boolean attribute
    mandatory: 7
",
        "For attribute 'mandatory', could not parse a(n) Boolean from '7'. Examples of valid data of this type: True, False", 7, 16);
    }

    [Fact]
    public void WrongDefaultValueType() {
      RunTest(@"
name: Tmp
description: Description
attributes:
  - name: myBoolean
    description: This is my boolean attribute
    dataType: Boolean
    default: 7
",
        "For attribute 'default', could not parse a(n) Boolean from '7'. Examples of valid data of this type: True, False", 8, 14);
    }

    private Entity RunTest(string yaml) {
      const string TMP_YAML_FILE = "Tmp.yaml";
      File.WriteAllText(TMP_YAML_FILE, yaml);
      ParserYaml parser = new ParserYaml();
      TreeNode rootNode = parser.Parse(TMP_YAML_FILE);
      rootNode.SetFileInfo(TMP_YAML_FILE);
      Assert.NotNull(rootNode);

      Entity entity = _compiler.CompileEntity(rootNode);
      ShowErrors();

      return entity;
    }

    private void RunTest(string yaml, string expectedErrorMessage, int expectedLine, int expectedChar) {
      RunTest(yaml);

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
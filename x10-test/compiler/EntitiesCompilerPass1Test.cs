using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

using Xunit;
using Xunit.Abstractions;

using x10.model.definition;
using x10.model.metadata;
using x10.parsing;

namespace x10.compiler {
  public class EntitiesCompilerPass1Test {

    private readonly ITestOutputHelper _output;
    private EntityCompilerPass1 _compiler = new EntityCompilerPass1(new MessageBucket());

    public EntitiesCompilerPass1Test(ITestOutputHelper output) {
      _output = output;
    }

    [Fact]
    public void CompileValidFile() {
      string correctYaml = File.ReadAllText("../../../compiler/data/Correct.yaml");
      Entity entity = RunTest(correctYaml);

      Assert.NotNull(entity);
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
        "The attribute 'description' should be a simple string of the correct type, but is a TreeHash", 3, 14);
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

    [Fact]
    public void WrongFormatForEntityName() {
      RunTest(@"
name: tmp
description: Description...
",
        "Invalid Entity name: 'tmp'. Must be upper-cased camel-case: e.g. 'User', 'PurchaseOrder'. Numbers are also allowed.", 2, 7);
    }

    [Fact]
    public void EntityNameDoesNotMatchFilename() {
      RunTest(@"
name: Blurg
description: Description...
",
        "The name of the entity 'Blurg' must match the name of the file: Tmp", 2, 7);
    }

    [Fact]
    public void WrongFormatForAttributeName() {
      RunTest(@"
name: Tmp
description: Description...

attributes: 
  - name: MyAttribute
    description: Description...
    dataType: String
",
        "Invalid Attribute name: 'MyAttribute'. Must be lower-case camel values: e.g. 'age', 'firstName'. Numbers are also allowed.", 6, 11);
    }

    [Fact]
    public void WrongFormatForAssociationName() {
      RunTest(@"
name: Tmp
description: Description...

associations: 
  - name: MyAssociation
    description: Description...
    dataType: Blurg
",
        "Invalid Association name: 'MyAssociation'. Must be lower-case camel values: e.g. 'sender', 'purchaseOrders'. Numbers are also allowed.", 6, 11);
    }

    [Fact]
    public void WrongFormatForEnumName() {
      RunTest(@"
name: Tmp
description: Description...

enums: 
  - name: myEnum
    description: Description...
",
        "Invalid Enum name: 'myEnum'. Must be upper-cased camel-case: e.g. 'Gender', 'CalendarMonths', 'EnrollmentState'. Numbers are also allowed.", 6, 11);
    }

    [Fact]
    public void WrongFormatForEnumValue() {
      RunTest(@"
name: Tmp
description: Description...

enums: 
  - name: MyEnum
    description: Description...
    values:
      - value: MyEnumValue
",
        "Invalid Enum value: 'MyEnumValue'. Must be lower-case camel values: e.g. 'male', 'awaitingApproval'. Numbers are also allowed.", 9, 16);
    }

    [Fact]
    public void BlankEnumValueLabel() {
      RunTest(@"
name: Tmp
description: Description...

enums: 
  - name: MyEnum
    description: Description...
    values:
      - value: myEnumValue
        label:
",
        "There should never be a blank enum value. If the value is optional, just make the attribute that uses it non-mandatory.", 10, 15);
    }

    [Fact]
    public void EnsureUniquenessOfEntityMemberNames() {
      RunTest(@"
name: Tmp
description: Description...

attributes: 
  - name: duplicate
    description: Description...
    dataType: String
  - name: unique
    description: Description...
    dataType: String

associations: 
  - name: duplicate
    description: Description...
    dataType: String
",
      "The name 'duplicate' is not unique among all the attributes and association of this Entity.", 6, 11);
    }

    [Fact]
    public void EnsureUniquenessOfEnumValues() {
      RunTest(@"
name: Tmp
description: Description...

enums: 
  - name: MyEnum
    description: Description...
    values:
      - value: duplicate
      - value: thisOneIsOk
      - value: duplicate
",
        "The value 'duplicate' is not unique among all the values of this Enum.", 9, 16);
    }

    [Fact(Skip = "Currently, YamlDotNet does not handle duplicate keys")]
    public void EnsureUniquenessOfAttributes() {
      RunTest(@"
name: Tmp
description: Description 1
description: Description 2
",
        "TODO", 3, 1);
    }

    private Entity RunTest(string yaml) {
      const string TMP_YAML_FILE = "Tmp.yaml";
      File.WriteAllText(TMP_YAML_FILE, yaml);
      ParserYaml parser = new ParserYaml();
      TreeNode rootNode = parser.Parse(TMP_YAML_FILE);
      rootNode.SetFileInfo(TMP_YAML_FILE);
      Assert.NotNull(rootNode);

      Entity entity = _compiler.CompileEntity(rootNode);
      TestUtils.DumpMessages(_compiler.Messages, _output);

      return entity;
    }

    private void RunTest(string yaml, string expectedErrorMessage, int expectedLine, int expectedChar) {
      RunTest(yaml);

      CompileMessage message = _compiler.Messages.Messages.FirstOrDefault(x => x.Message == expectedErrorMessage);
      Assert.NotNull(message);

      Assert.Equal(expectedLine, message.TreeElement.Start.LineNumber);
      Assert.Equal(expectedChar, message.TreeElement.Start.CharacterPosition);
    }
  }
}
﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

using Xunit;
using Xunit.Abstractions;

using x10.parsing;
using x10.model;
using x10.model.definition;
using x10.model.metadata;

namespace x10.compiler {
  public class EntitiesCompilerPass1Test {

    private readonly ITestOutputHelper _output;
    private readonly MessageBucket _messages = new MessageBucket();
    private readonly EntityCompilerPass1 _compiler;

    public EntitiesCompilerPass1Test(ITestOutputHelper output) {
      _output = output;

      AttributeReader attrReader = new AttributeReader(_messages);
      EnumsCompiler enumsCompiler = new EnumsCompiler(_messages, new AllEnums(_messages), attrReader);
      _compiler = new EntityCompilerPass1(_messages, enumsCompiler, attrReader);
    }

    [Fact]
    public void CompileValidFile() {
      string correctYaml = File.ReadAllText("../../../compiler/model/data/Correct.yaml");
      Entity entity = RunTest(correctYaml);

      Assert.NotNull(entity);
      Assert.Equal(0, _messages.Count);
    }

    [Fact]
    public void EntityNameDoesNotMatchFilename() {
      ParserYaml parser = new ParserYaml(_messages, "../../../compiler/model/data");
      List<IParseElement> parsed = parser.RecursivelyParseDirectory();
      TreeNode badFilenameRoot = (TreeNode)parsed.Single(x => x.FileInfo.FileName == "BadFileName.yaml");

      Entity entity = _compiler.CompileEntity(badFilenameRoot);
      TestUtils.DumpMessages(_messages, _output);
      Assert.NotNull(entity);

      VerifyErrorMessage("The name of the entity 'Tmp' must match the name of the file: BadFileName", 1, 7);
    }

    [Fact]
    public void CompileWithSetterAndNonSetter() {
      ModelAttributeDefinitions.All.Add(new ModelAttributeDefinitionAtomic() {
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
      Assert.Equal("My custom value", entity.FindValue("customField"));
    }

    [Fact]
    public void WrongRootNodeType() {
      RunTest(@"
- one
- two
",
        "The root node of an Entity file must be a Hash, but was: TreeSequence", 2, 1);
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
        "Mandatory attribute 'values' is missing", 5, 5);
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
    public void MissingAttributeWarning() {
      RunTest(@"
name: Tmp
",
        "The attribute 'description' is missing from Entity", 2, 1);
    }

    [Fact]
    public void MissingAttributeError() {
      RunTest(@"
name: Tmp
description: Desc
attributes:
  - name: attr
    description: Desc
    dataType: Integer
derivedAttributes:
  - name: derived
    description: Desc
    formula: =attr + 1
",
        "The attribute 'dataType' is missing from DerivedAttribute", 9, 5);
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
    dataType: Boolean
    mandatory: 7
",
        "Error parsing attribute 'mandatory': could not parse a(n) Boolean from '7'. Examples of valid data of this type: True, False.", 8, 16);
    }

    [Fact]
    public void WrongFormatForEntityName() {
      RunTest(@"
name: tmp
description: Description...
",
        "Invalid Entity name: 'tmp'. Must be upper-cased CamelCase: e.g. 'User', 'PurchaseOrder'. Numbers are also allowed.", 2, 7);
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
        "Invalid Attribute name: 'MyAttribute'. Must be lower-cased camelCase or snake_case: e.g. 'age', 'firstName', 'first_name'. Numbers are also allowed.", 6, 11);
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
        "Invalid Association name: 'MyAssociation'. Must be lower-cased camelCase or snake_case: e.g. 'sender', 'purchaseOrders', 'purchase_orders'. Numbers are also allowed.", 6, 11);
    }

    [Fact]
    public void WrongFormatForEnumName() {
      RunTest(@"
name: Tmp
description: Description...

enums: 
  - name: myEnum
    description: Description...
    values:
      - value: one
",
        "Invalid Enum name: 'myEnum'. Must be upper-cased CamelCase: e.g. 'Gender', 'CalendarMonths', 'EnrollmentState'. Numbers are also allowed.", 6, 11);
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
        "Invalid Enum value: 'MyEnumValue'. Must be lower-cased camelCase or ALL_CAPS: e.g. 'male', 'awaitingApproval, ASAP'. Numbers are also allowed.", 9, 16);
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
        "???", 3, 1);
    }

    private Entity RunTest(string yaml) {
      ParserYaml parser = new ParserYaml(_messages, null);
      TreeNode rootNode = parser.ParseFromString(yaml);
      Assert.NotNull(rootNode);

      Entity entity = _compiler.CompileEntity(rootNode);
      TestUtils.DumpMessages(_messages, _output);

      return entity;
    }

    private void RunTest(string yaml, string expectedErrorMessage, int expectedLine, int expectedChar) {
      RunTest(yaml);
      VerifyErrorMessage(expectedErrorMessage, expectedLine, expectedChar);
    }

    private void VerifyErrorMessage(string expectedErrorMessage, int expectedLine, int expectedChar) {
      CompileMessage message = _messages.Messages.FirstOrDefault(x => x.Message == expectedErrorMessage);
      Assert.NotNull(message);

      Assert.Equal(expectedLine, message.ParseElement.Start.LineNumber);
      Assert.Equal(expectedChar, message.ParseElement.Start.CharacterPosition);
    }
  }
}
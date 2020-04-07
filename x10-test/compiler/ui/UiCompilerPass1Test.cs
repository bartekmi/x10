using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

using Xunit;
using Xunit.Abstractions;

using x10.parsing;
using x10.model;
using x10.model.definition;
using x10.model.metadata;
using x10.ui.metadata;
using x10.ui.composition;

namespace x10.compiler {
  public class UiCompilerPass1Test {

    private readonly ITestOutputHelper _output;
    private readonly MessageBucket _messages = new MessageBucket();
    private readonly UiCompilerPass1 _compiler;

    public UiCompilerPass1Test(ITestOutputHelper output) {
      _output = output;

      AllEnums allEnums = new AllEnums(_messages);
      AllEntities allEntities = CreateEntities(allEnums);
      UiAttributeReader attrReader = new UiAttributeReader(_messages, allEntities, allEnums);
      _compiler = new UiCompilerPass1(_messages, attrReader);
    }

    private AllEntities CreateEntities(AllEnums allEnums) {
      // At this point, allEnums is unused
      AllEntities allEntities = TestUtils.EntityCompile(_messages, new string[] {
        @"
name: Building
description: dummy
attributes:
  - name: name
    description: dummy
    dataType: String
  - name: apartmentCount
    description: dummy
    dataType: Integer
associations:
  - name: apartments
    description: dummy
    dataType: Apartment
    many: true
",
        @"
name: Apartment
description: dummy
attributes:
  - name: number
    description: dummy
    dataType: Integer
  - name: squreFootage
    description: dummy
    dataType: Float
",
      });

      if (_messages.HasErrors) {
        TestUtils.DumpMessages(_messages, _output, CompileMessageSeverity.Error);
        throw new Exception("Entities did not load cleanly - see output");
      }

      return allEntities;
    }

    [Fact]
    public void CompileSuccess() {
      UiDefinitionX10 definition = RunTest(@"
<MyComponent description='My description...' model='Building'>
  <VerticalGroup>
    <name/>
    <apartmentCount ui='MyFunkyIntComponent'/>
    <Table path='apartments'/>
  </VerticalGroup>
</MyComponent>
");

      Assert.Equal("MyComponent", definition.Name);
      Assert.Equal("My description...", definition.Description);
      // TODO: Verify data as expected
    }

    [Fact]
    public void CompileWithSetterAndNonSetter() {
      UiAttributeDefinitions.All.Add(new UiAttributeDefinitionPrimitive() {
        Name = "customField",
        Description = "This is a custom field with no setter",
        AppliesTo = UiAppliesTo.UiDefinition,
        DataType = DataTypes.Singleton.String,
      });

      UiDefinitionX10 definition = RunTest(@"
<MyComponent description='My description...' model='Building' customField='My custom value'/>
");

      Assert.False(_messages.HasErrors);
      Assert.Equal("MyComponent", definition.Name);
      Assert.Equal("My description...", definition.Description);
      Assert.Equal("My custom value", UiAttributeUtils.FindValue(definition, "customField"));
    }

    [Fact]
    public void MissingAttribute() {
      RunTest(@"
<MyComponent description='My description...' />
",
        "The attribute 'model' is missing from UiDefinition", 2, 2);
    }

    [Fact]
    public void WrongTypeOfAttribute() {
      RunTest(@"
<MyComponent many='7'/>
",
        "Error parsing attribute 'many': could not parse a(n) Boolean from '7'. Examples of valid data of this type: True, False.", 2, 14);
    }

    [Fact]
    public void WrongFormatForUiComponentName() {
      RunTest(@"
<myComponent/>
",
        "Invalid UI Element name: 'myComponent'. Must be upper-cased CamelCase: e.g. 'DropDown', 'TextArea'. Numbers are also allowed.", 2, 2);
    }

    [Fact]
    public void EntityNameDoesNotMatchFilename() {
      RunTest(@"
<MyOtherComponent/>
",
        "The name of the UI Component 'MyOtherComponent' must match the name of the file: MyComponent", 2, 2);
    }

    #region Utilities
    private UiDefinitionX10 RunTest(string xml, string fileName = "MyComponent.xml") {
      ParserXml parser = new ParserXml(_messages);
      XmlElement rootNode = parser.ParseFromString(xml, fileName);

      if (rootNode == null) {
        TestUtils.DumpMessages(_messages, _output);
        Assert.NotNull(rootNode);
      }

      UiDefinitionX10 definition = _compiler.CompileUiDefinition(rootNode);

      TestUtils.DumpMessages(_messages, _output);

      return definition;
    }

    private void RunTest(string xml, string expectedErrorMessage, int expectedLine, int expectedChar) {
      RunTest(xml);

      CompileMessage message = _messages.Messages.FirstOrDefault(x => x.Message == expectedErrorMessage);
      Assert.NotNull(message);

      Assert.Equal(expectedLine, message.ParseElement.Start.LineNumber);
      Assert.Equal(expectedChar, message.ParseElement.Start.CharacterPosition);
    }
    #endregion
  }
}
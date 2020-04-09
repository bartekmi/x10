using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

using Xunit;
using Xunit.Abstractions;

using x10.parsing;
using x10.model.definition;
using x10.model.metadata;
using x10.model;
using x10.compiler;
using x10.ui.composition;
using x10.ui.metadata;

namespace x10.compiler {
  public class UiCompilerPass2Test : UiCompilerTestBase {

    #region Test Setup
    private UiLibrary _library;

    public UiCompilerPass2Test(ITestOutputHelper output) : base(output) {
      List<ClassDef> definitions = new List<ClassDef>() {
        new ClassDefNative() {
          Name = "VerticalGroup",
          AttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionComplex() {
              IsPrimary = true,
              Name = "Children",
              IsMany = true,
            },
          },
        },
        new ClassDefNative() {
          Name = "Table",
          AttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionComplex() {
              IsPrimary = true,
              Name = "Columns",
              IsMany = true,
            },
            new UiAttributeDefinitionComplex() {
              Name = "Header",
              IsMany = true,
            },
          },
        },
        new ClassDefNative() {
          Name = "TableColumn",
          AttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionComplex() {
              IsPrimary = true,
              Name = "Renderer",
            },
            new UiAttributeDefinitionAtomic() {
              Name = "label",
              DataType = DataTypes.Singleton.String,
            },
          },
        },
        new ClassDefNative() {
          Name = "MyFunkyIntComponent",
          AttributeDefinitions = new List<UiAttributeDefinition>(),
        },
        new ClassDefNative() {
          Name = "Button",
          AttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionAtomic() {
              Name = "label",
              IsMandatory = true,
              DataType = DataTypes.Singleton.String,
            },
            new UiAttributeDefinitionAtomic() {
              Name = "action",
              IsMandatory = true,
              DataType = DataTypes.Singleton.String,
            },
          },
        },
      };

      _library = new UiLibrary(definitions) {
        Name = "Test Library",
      };
    }
    #endregion


    [Fact]
    public void CompileSuccessPass2_1() {
      ClassDefX10 definition = RunTest(@"
<MyComponent description='My description...' model='Building' many='true'>
  <VerticalGroup>
    <name/>
    <apartmentCount ui='MyFunkyIntComponent'/>
    <Table path='apartments.rooms'>
      <Table.Header>
        <HelpIcon text='A useful help message...'/>
      </Table.Header>
      <name/>
      <squareFootage/>
      <TableColumn label='View Image'>
        <Button label='View Image' action='TODO'/>
      </TableColumn>
    </Table>
  </VerticalGroup>
</MyComponent>
");

      Assert.Equal("MyComponent", definition.Name);
      Assert.Equal("My description...", definition.Description);
      Assert.Same(_allEntities.FindEntityByName("Building"), definition.ComponentDataModel);
      Assert.True(definition.IsMany);

      InstanceClassDefUse verticalGroup = (InstanceClassDefUse)definition.RootChild;
      Assert.Equal("VerticalGroup", UiAttributeUtils.FindValue(verticalGroup, ParserXml.ELEMENT_NAME));
      List<Instance> vGroupChildren = ((UiAttributeValueComplex)verticalGroup.PrimaryValue).Instances;
      Assert.Equal(3, vGroupChildren.Count);

      InstanceModelRef apartmentCount = (InstanceModelRef)vGroupChildren[1];
      Assert.Equal("apartmentCount", apartmentCount.Path);
      Assert.Equal("MyFunkyIntComponent", UiAttributeUtils.FindValue(apartmentCount, "ui"));

      InstanceClassDefUse table = (InstanceClassDefUse)vGroupChildren[2];
      Assert.Equal("Table", UiAttributeUtils.FindValue(table, ParserXml.ELEMENT_NAME));
      Assert.Equal("apartments.rooms", table.Path);
    }

    [Fact]
    public void CompileSuccess() {
      ClassDefX10 outer = CompilePass1(@"
<Outer description='My description...' model='Building'>
  <VerticalGroup>
    <name/>
    <apartmentCount ui='MyFunkyIntComponent'/>
    <Inner path='apartments'/>
  </VerticalGroup>
</Outer>
");

      ClassDefX10 inner = CompilePass1(@"
<Inner description='My description...' model='Apartment' many='true'>
  <Table>
  </Table>
</Inner>
");

      CompilePass2(inner, outer);
      Assert.Empty(_messages.Messages);
    }

    [Fact]
    public void NonExistentComponentUse() {
      RunTest(@"
<Outer description='My description...' model='Building'>
  <VerticalGroup>
    <Bogus/>
  </VerticalGroup>
</Outer>
", "UI Component 'Bogus' not found", 4, 6);
    }

    [Fact]
    public void BadModelReference() {
      RunTest(@"
<Outer description='My description...' model='Building'>
  <Table path='apartments'>
    <nonExistent/>
  </Table>
</Outer>
", "Member nonExistent does not exist on Entity Apartment.", 4, 6);
    }

    #region Utilities

    private ClassDefX10 CompilePass1(string xml) {
      ClassDefX10 definition = TestUtils.UiCompilePass1(xml, _messages, _compilerPass1, _output);
      return definition;
    }

    private void CompilePass2(params ClassDefX10[] uiDefinitions) {
      TestUtils.UiCompilePass2(_messages, _allEntities, _allEnums, _library, uiDefinitions);
      TestUtils.DumpMessages(_messages, _output);
    }

    private void RunTest(string xml, string expectedErrorMessage, int expectedLine, int expectedChar) {
      ClassDefX10 definiton = CompilePass1(xml);
      RunTest(expectedErrorMessage, expectedLine, expectedChar, definiton);
    }

    private void RunTest(string expectedErrorMessage, int expectedLine, int expectedChar, params ClassDefX10[] definitions) {
      CompilePass2(definitions);

      CompileMessage message = _messages.Messages.FirstOrDefault(x => x.Message == expectedErrorMessage);
      Assert.NotNull(message);

      Assert.Equal(expectedLine, message.ParseElement.Start.LineNumber);
      Assert.Equal(expectedChar, message.ParseElement.Start.CharacterPosition);
    }

    private ClassDefX10 RunTest(string xml) {
      ClassDefX10 definition = CompilePass1(xml);
      CompilePass2(definition);

      return definition;
    }


    #endregion
  }
}
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

using Xunit;
using Xunit.Abstractions;

using x10.parsing;
using x10.model.metadata;
using x10.ui.composition;
using x10.ui.metadata;
using x10.model.definition;

namespace x10.compiler {
  public class UiCompilerPass2Test : UiCompilerTestBase {

    #region Test Setup
    private UiLibrary _library;

    public UiCompilerPass2Test(ITestOutputHelper output) : base(output) {
      List<ClassDef> definitions = new List<ClassDef>() {
        // Basic Components
        new ClassDefNative() {
          Name = "MyFunkyIntComponent",
          IsMany = false,
          DataModelType = DataModelType.Scalar,
        },
        new ClassDefNative() {
          Name = "MyAverageIntComponent",
          IsMany = false,
          DataModelType = DataModelType.Scalar,
        },
        new ClassDefNative() {
          Name = "MyBasicIntComponent",
          IsMany = false,
          DataModelType = DataModelType.Scalar,
        },
        new ClassDefNative() {
          Name = "TextEdit",
          IsMany = false,
          DataModelType = DataModelType.Scalar,
        },

        // Complex Components
        new ClassDefNative(new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionComplex() {
              IsPrimary = true,
              Name = "Children",
              IsMany = true,
            },
          }) {
          Name = "VerticalGroup",
        },
        new ClassDefNative(new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionComplex() {
              IsPrimary = true,
              Name = "Columns",
              IsMany = true,
              IsMandatory = true,
              ReducesManyToOne = true,
            },
            new UiAttributeDefinitionComplex() {
              Name = "Header",
              IsMandatory = true,
            },
          }) {
          Name = "Table",
          IsMany = true,
          DataModelType = DataModelType.Entity,
        },
        new ClassDefNative(new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionComplex() {
              IsPrimary = true,
              Name = "Renderer",
            },
            new UiAttributeDefinitionAtomic() {
              Name = "label",
              DataType = DataTypes.Singleton.String,
            },
          }) {
          Name = "TableColumn",
        },
        new ClassDefNative(new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionAtomic() {
              Name = "text",
              DataType = DataTypes.Singleton.String,
            },
          }) {
          Name = "HelpIcon",
        },
        new ClassDefNative(new List<UiAttributeDefinition>() {
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
          }) {
          Name = "Button",
        },
        new ClassDefNative() {
          Name = "RoomViewer3D",
          ComponentDataModel = _allEntities.FindEntityByName("Room"),
          DataModelType = DataModelType.Entity,
          IsMany = false,
        },
      };

      _library = new UiLibrary(definitions) {
        Name = "Test Library",
      };

      _library.AddDataTypeToComponentAssociation(DataTypes.Singleton.Integer, "MyBasicIntComponent");
      _library.AddDataTypeToComponentAssociation(DataTypes.Singleton.String, "TextEdit");
    }
    #endregion

    #region Pass 2.1

    #region Success
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

      // There are errors, but we don't care about them for this test (Only testing compile 2.1)
      string result = Print(definition);

      Assert.Equal(@"<MyComponent description='My description...' model='Building' many='True'>
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
", result);
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
    <Table.Header>
      <Button label='Hi' action='doSomething'/>
    </Table.Header>
    <number/>
  </Table>
</Inner>
");

      CompilePass2(inner, outer);
      Assert.Empty(_messages.Messages);

      Assert.Equal(@"<Outer description='My description...' model='Building'>
  <VerticalGroup>
    <name/>
    <apartmentCount ui='MyFunkyIntComponent'/>
    <Inner path='apartments'/>
  </VerticalGroup>
</Outer>
", Print(outer));

      Assert.Equal(@"<Inner description='My description...' model='Apartment' many='True'>
  <Table>
    <Table.Header>
      <Button label='Hi' action='doSomething'/>
    </Table.Header>
    <number/>
  </Table>
</Inner>
", Print(inner));
    }
    #endregion

    #region Errors
    [Fact]
    public void NonExistentComponentUse() {
      RunTest(@"
<Outer description='My description...' model='Building'>
  <VerticalGroup>
    <Bogus/>
  </VerticalGroup>
</Outer>
", "UI Component 'Bogus' not found", 4, 6);

      Assert.Single(_messages.Messages);
    }

    [Fact]
    public void ExpectingInstanceButDidNotGetOne() {
      RunTest(@"
<Outer description='My description...' model='Building'>
  <Outer.Property/>
</Outer>
", "Expecting either a Model Reference (e.g. <name\\>) or a Component Reference (e.g. <TextField path='name'\\> but got neither.", 3, 4);

      Assert.Single(_messages.Messages);
    }

    [Fact]
    public void EmptyComplexAttribute() {
      RunTest(@"
<Outer description='My description...' model='Building' many='true'>
  <Table>
    <Table.Header>
    </Table.Header>
    <name/>
  </Table>
</Outer>
", "Empty Complex Attribute", 4, 6);

      Assert.Equal(2, _messages.Messages.Count);
    }

    [Fact]
    public void BadComplexAttribute() {
      RunTest(@"
<Outer description='My description...' model='Building' many='true'>
  <Table>
    <Table.Bogus>
    </Table.Bogus>
    <Table.Header>
      <HelpIcon/>
    </Table.Header>
    <name/>
  </Table>
</Outer>
", "Complex Attribute 'Bogus' does not exist on Component 'Table'", 4, 6);

      Assert.Single(_messages.Messages);
    }

    [Fact]
    public void NonComplexAttributeWhereComplexExpected() {
      RunTest(@"
<Outer description='My description...' model='Building' many='true'>
  <Table>
    <TableColumn>
      <TableColumn.label/>
    </TableColumn>
    <Table.Header>
      <HelpIcon/>
    </Table.Header>
  </Table>
</Outer>
", "Atomic Attribute 'label' of Component 'TableColumn' found where Complex Attribute expected.", 5, 8);

      Assert.Single(_messages.Messages);
    }
    #endregion
    #endregion

    #region Pass 2.2 - Path Resolution, Model Ref UI Resolution

    #region Success - Path Resolution
    [Fact]
    public void PathResolution() {
      ClassDefX10 definition = RunTest(@"
<MyComponent model='Building'>
  <VerticalGroup>       <!-- No path -->
    <name/>             <!-- Path for a Model Reference -->
    <Table path='apartments.rooms'>   <!-- double member path-->
      <Table.Header>
        <HelpIcon/>
      </Table.Header>
      <name/>
      <TableColumn>
        <Button path='paintColor' label='Boo' action='doSomething'/>    <!-- Many to single -->
      </TableColumn>
    </Table>
  </VerticalGroup>
</MyComponent>
");

      Assert.Empty(_messages.Messages);
      string result = Print(definition, new PrintConfig() { AlwaysPrintPath = true });

      Assert.Equal(@"<MyComponent model='Building'>
  <VerticalGroup path=''>
    <name path='name'/>
    <Table path='apartments.rooms'>
      <Table.Header>
        <HelpIcon path=''/>
      </Table.Header>
      <name path='name'/>
      <TableColumn path=''>
        <Button path='paintColor' label='Boo' action='doSomething'/>
      </TableColumn>
    </Table>
  </VerticalGroup>
</MyComponent>
", result);
    }
    #endregion

    #region Path-Related Errors
    [Fact]
    public void NonExistentPathMember() {
      RunTest(@"
<Outer model='Building'>
  <VerticalGroup path='bogus'>
    <Button path='doubleBogus' label='Boo' action='doSomething'/>   <!-- Does not get here -->
  </VerticalGroup>
</Outer>
", "Member 'bogus' does not exist on Entity Building.", 3, 18);

      Assert.Single(_messages.Messages);
    }

    [Fact]
    public void NonExistentNestedPathMember() {
      RunTest(@"
<Outer model='Building'>
  <VerticalGroup path='apartments.windows'/>
</Outer>
", "Member 'windows' does not exist on Entity Apartment.", 3, 18);

      Assert.Single(_messages.Messages);
    }

    [Fact]
    public void BadModelReference() {
      RunTest(@"
<Outer description='My description...' model='Building' many='true'>
  <Table path='apartments'>
    <Table.Header>
      <HelpIcon/>
    </Table.Header>
    <nonExistent/>
  </Table>
</Outer>
", "Member 'nonExistent' does not exist on Entity Apartment.", 7, 6);

      Assert.Single(_messages.Messages);
    }
    #endregion

    #region Attribute Errors - Missing Mandatory and Unknown
    [Fact]
    public void MissingMandatoryAttributes() {
      RunTest(@"
<Outer description='My description...' model='Building' many='true'>
  <VerticalGroup>
    <Table/>
    <Button/>
  </VerticalGroup>
</Outer>
",
      "Mandatory Primary Attribute 'Columns' of Class Definition 'Table' is missing",
      "Mandatory Complex Attribute 'Header' of Class Definition 'Table' is missing",
      "Mandatory Atomic Attribute 'label' of Class Definition 'Button' is missing",
      "Mandatory Atomic Attribute 'action' of Class Definition 'Button' is missing");

      Assert.Equal(4, _messages.Messages.Count);
    }

    [Fact]
    public void UnknownAttribute() {
      RunTest(@"
<Outer description='My description...' model='Building'>
  <VerticalGroup>
    <name foo='some value'/>
    <HelpIcon bar='blurg'/>
  </VerticalGroup>
</Outer>
",
      "Unknown attribute 'foo'",
      "Unknown attribute 'bar'");

      Assert.Equal(2, _messages.Messages.Count);
    }
    #endregion

    #region ValidateDataModelCompatibility
    [Fact]
    public void WrongEntityType() {
      RunTest(@"
<Outer description='My description...' model='Building' many='true'>
  <Table path='apartments'>
    <TableColumn>
      <RoomViewer3D/>
    </TableColumn>
    <Table.Header>
      <HelpIcon/>
    </Table.Header>
  </Table>
</Outer>
", "Data Type mismatch. Component RoomViewer3D expects Entity 'Room', but the path is delivering Entity 'Apartment'", 5, 8);

      Assert.Single(_messages.Messages);
    }

    [Fact]
    public void ExpectedManyGotOne() {
      RunTest(@"
<Outer model='Building'>
  <Table>
    <Table.Header>
      <HelpIcon/>
    </Table.Header>
    <name/>
  </Table>
</Outer>
", "The component Table expects MANY Entities, but the path is delivering a SINGLE 'Building' Entity", 3, 4);

      Assert.Single(_messages.Messages);
    }

    [Fact]
    public void ExpectedOneGotMany() {
      RunTest(@"
<Outer model='Building' many='true'>
  <MyFunkyIntComponent path='apartmentCount'/>
</Outer>
", "The component MyFunkyIntComponent expects a SINGLE value, but the path is delivering MANY 'Building.apartmentCount' values", 3, 4);

      Assert.Single(_messages.Messages);
    }
    #endregion

    #region Model Ref UI Component Resolution

    [Fact]
    public void ResolveModelReferenceUiComponent() {
      ClassDefX10 definition = RunTest(@"
<Outer description='My description...' model='Building'>
  <VerticalGroup>
    <apartmentCount/>
    <apartmentCount ui='MyFunkyIntComponent'/>
    <ageInYears/>
    <ageInYears ui='MyFunkyIntComponent'/>
  </VerticalGroup>
</Outer>
");

      Assert.Empty(_messages.Messages);

      string result = Print(definition, new PrintConfig() { AlwaysPrintRenderAs = true });
      Assert.Equal(@"<Outer description='My description...' model='Building'>
  <VerticalGroup>
    <apartmentCount renderAs='MyBasicIntComponent'/>
    <apartmentCount ui='MyFunkyIntComponent' renderAs='MyFunkyIntComponent'/>
    <ageInYears renderAs='MyAverageIntComponent'/>
    <ageInYears ui='MyFunkyIntComponent' renderAs='MyFunkyIntComponent'/>
  </VerticalGroup>
</Outer>
", result);

    }
    #endregion

    #endregion

    #region Utilities

    private string Print(ClassDefX10 classDef, PrintConfig config = null) {
      _output.WriteLine("");

      using (StringWriter writer = new StringWriter()) {
        classDef.Print(writer, 0, config);
        _output.WriteLine(writer.ToString());
        return writer.ToString();
      }
    }

    private ClassDefX10 CompilePass1(string xml) {
      ClassDefX10 definition = TestUtils.UiCompilePass1(xml, _messages, _compilerPass1, _output);
      return definition;
    }

    private void CompilePass2(params ClassDefX10[] uiDefinitions) {
      TestUtils.UiCompilePass2(_messages, _allEntities, _allEnums, _library, uiDefinitions);
      TestUtils.DumpMessages(_messages, _output);
    }

    private void RunTest(string xml, string expectedErrorMessage, int expectedLine, int expectedChar) {
      ClassDefX10 definition = CompilePass1(xml);
      RunTest(expectedErrorMessage, expectedLine, expectedChar, definition);
    }

    private void RunTest(string xml, params string[] expectedErrorMessages) {
      ClassDefX10 definition = CompilePass1(xml);
      CompilePass2(new ClassDefX10[] { definition });

      foreach (string expectedErrorMessage in expectedErrorMessages) {
        CompileMessage message = _messages.Messages.FirstOrDefault(x => x.Message == expectedErrorMessage);
        if (message == null)
          _output.WriteLine("Did not find: " + expectedErrorMessage);
        Assert.NotNull(message);
      }
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

      AllUiDefinitions allUiDefinitions = new AllUiDefinitions(_messages, null, _library);
      foreach (Entity entity in _allEntities.All)
        (new EntityCompilerPass3(allUiDefinitions)).CompileEntity(entity);

      CompilePass2(definition);

      return definition;
    }


    #endregion
  }
}
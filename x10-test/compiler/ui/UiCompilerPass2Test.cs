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
    private readonly UiLibrary _library;

    public UiCompilerPass2Test(ITestOutputHelper output) : base(output) {
      List<ClassDef> definitions = new List<ClassDef>() {
        #region Basic Components
        new ClassDefNative() {
          Name = "MyFunkyIntComponent",
          AtomicDataModel = DataTypes.Singleton.Integer,
          IsMany = false,
          InheritsFrom = ClassDefNative.Visual,
        },
        new ClassDefNative() {
          Name = "MyAverageIntComponent",
          AtomicDataModel = DataTypes.Singleton.Integer,
          IsMany = false,
          InheritsFrom = ClassDefNative.Visual,
        },
        new ClassDefNative() {
          Name = "MyBasicIntComponent",
          AtomicDataModel = DataTypes.Singleton.Integer,
          IsMany = false,
          InheritsFrom = ClassDefNative.Visual,
        },
        new ClassDefNative() {
          Name = "TextEdit",
          AtomicDataModel = DataTypes.Singleton.String,
          IsMany = false,
          InheritsFrom = ClassDefNative.Visual,
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionAtomic() {
              Name = "weight",
              DataType = new DataTypeEnum() {
                Name = "FontWeight",
                EnumValueValues = new string[] { "normal", "bold" },
              }
            },
          }
        },
        new ClassDefNative() {
          Name = "Checkbox",
          AtomicDataModel = DataTypes.Singleton.Boolean,
          IsMany = false,
          InheritsFrom = ClassDefNative.Visual,
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionAtomic() {
              Name = "checked",
              DataType = DataTypes.Singleton.Boolean,
            },
          }
        },
        new ClassDefNative() {
          Name = "DropDown",
          InheritsFrom = ClassDefNative.Visual,
          IsMany = false,
          AtomicDataModel = new DataTypeEnum(),
        },
        #endregion

        #region Complex Components
        new ClassDefNative() {
          Name = "VerticalGroup",
          InheritsFrom = ClassDefNative.Visual,
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionComplex() {
              IsPrimary = true,
              Name = "Children",
              IsMany = true,
              ComplexAttributeType = ClassDefNative.Visual,
            },
          }
        },
        new ClassDefNative() {
          Name = "Table",
          ComponentDataModel = Entity.Object,
          IsMany = true,
          InheritsFrom = ClassDefNative.Visual,
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionComplex() {
              IsPrimary = true,
              Name = "Columns",
              IsMany = true,
              IsMandatory = true,
              ReducesManyToOne = true,
              ComplexAttributeTypeName = "TableColumn",
              ModelRefWrapperComponentName = "TableColumn",
            },
            new UiAttributeDefinitionComplex() {
              Name = "Header",
              IsMandatory = true,
              ComplexAttributeType = ClassDefNative.Visual,
            },
            new UiAttributeDefinitionAtomic() {
              Name = "selected",
              Description = "Read/Write list of items which are currently selected",
              IsMany = true,
              DataType = DataTypes.Singleton.String,
            },
          },
        },
        new ClassDefNative() {
          Name = "TableColumn",
          InheritsFrom = ClassDefNative.Object,
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionComplex() {
              IsPrimary = true,
              Name = "Renderer",
              ComplexAttributeType = ClassDefNative.Visual,
            },
            new UiAttributeDefinitionAtomic() {
              Name = "label",
              DataType = DataTypes.Singleton.String,
            },
            new UiAttributeDefinitionAtomic() {
              Name = "sortable",
              DataType = DataTypes.Singleton.Boolean,
              DefaultValue = true,
            },
          },
        },
        new ClassDefNative() {
          Name = "HelpIcon",
          InheritsFrom = ClassDefNative.Visual,
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionAtomic() {
              Name = "text",
              DataType = DataTypes.Singleton.String,
            },
            new UiAttributeDefinitionAtomic() {
              Name = "difficulty",
              DataType = DataTypes.Singleton.Integer,
            },
          },
        },
        new ClassDefNative() {
          Name = "Button",
          InheritsFrom = ClassDefNative.Visual,
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
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

        #endregion

        #region With Attached Property
        new ClassDefNative() {
          Name = "WithAttached",
          InheritsFrom = ClassDefNative.Visual,
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionAtomic() {
              Name = "myAttached",
              DataType = DataTypes.Singleton.Integer,
              IsAttached = true,
            },
            new UiAttributeDefinitionComplex() {
              IsPrimary = true,
              Name = "Children",
              IsMany = true,
              ComplexAttributeType = ClassDefNative.Visual,
            },
          },
        },
        #endregion

        #region Application Specific
        new ClassDefNative() {
          Name = "RoomViewer3D",
          ComponentDataModel = _allEntities.FindEntityByName("Room"),
          IsMany = false,
          InheritsFrom = ClassDefNative.Visual,
        },
        #endregion 

        ClassDefNative.RawHtml,
        ClassDefNative.State,
      };

      _library = new UiLibrary(definitions) {
        Name = "Test Library",
      };

      _library.AddDataTypeToComponentAssociation(DataTypes.Singleton.Integer, "MyBasicIntComponent");
      _library.AddDataTypeToComponentAssociation(DataTypes.Singleton.String, "TextEdit");
      _library.AddDataTypeToComponentAssociation(DataTypes.Singleton.Boolean, "Checkbox");
      _library.SetComponentForEnums("DropDown");

      if (_library.HydrateAndValidate(_messages)) {
        TestUtils.DumpMessages(_messages, _output);
        Assert.Empty(_messages.Messages);
      }
    }
    #endregion

    #region Pass 2.1

    #region Success
    [Fact]
    public void CompileSuccessPass2_1() {
      ClassDefX10 definition = RunTest(@"
<MyComponent description='My description...' model='Building'>
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

      Assert.Equal(@"<MyComponent description='My description...' model='Building'>
  <VerticalGroup>
    <name/>
    <apartmentCount ui='MyFunkyIntComponent'/>
    <Table path='apartments.rooms'>
      <Table.Header>
        <HelpIcon text='A useful help message...'/>
      </Table.Header>
      <TableColumn>
        <name/>
      </TableColumn>
      <TableColumn>
        <squareFootage/>
      </TableColumn>
      <TableColumn label='View Image'>
        <Button label='View Image' action='TODO'/>
      </TableColumn>
    </Table>
  </VerticalGroup>
</MyComponent>
", result);
    }

    [Fact]
    public void TableColumnWithRichContent() {
      ClassDefX10 definition = RunTest(@"
<MyComponent description='My description...' model='Building' many='True'>
  <Table>
    <Table.Header>
      <HelpIcon text='A useful help message...'/>
    </Table.Header>
    <TableColumn>
      <VerticalGroup>
        <name/>
        <ageInYears/>
      </VerticalGroup>
    </TableColumn>
  </Table>
</MyComponent>
");

      // There are errors, but we don't care about them for this test (Only testing compile 2.1)
      string result = Print(definition);

      Assert.Equal(@"<MyComponent description='My description...' model='Building' many='True'>
  <Table>
    <Table.Header>
      <HelpIcon text='A useful help message...'/>
    </Table.Header>
    <TableColumn>
      <VerticalGroup>
        <name/>
        <ageInYears/>
      </VerticalGroup>
    </TableColumn>
  </Table>
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
    <TableColumn>
      <number/>
    </TableColumn>
  </Table>
</Inner>
", Print(inner));
    }

    [Fact]
    public void RawHtmlSpecialComponent() {
      ClassDefX10 definition = RunTest(@"
<MyComponent description='My description...'>
  <RawHtml>
    <p> This should be <b>ignored</b> by the compiler</p>
  </RawHtml>
</MyComponent>
");

      Assert.Empty(_messages.Messages);
      string result = Print(definition);

      Assert.Equal(@"<MyComponent description='My description...'>
  <RawHtml/>
</MyComponent>
", result);
    }

    [Fact]
    public void StateDefinition() {
      ClassDefX10 definition = RunTest(@"
<MyComponent description='My description...' model='Building'>
  <MyComponent.State>
    <State variable='myVar1' dataType='String' default='Hello World' />
    <State variable='myVar2' dataType='Boolean' default='true' />
    <State variable='myVar3' dataType='Date' />
  </MyComponent.State>
  <VerticalGroup>
    <Checkbox checked='=myVar2'/> 
    <name visible='=myVar2'/>
  </VerticalGroup>
</MyComponent>
");

      Assert.Empty(_messages.Messages);
      string result = Print(definition);

      Assert.Equal(@"<MyComponent description='My description...' model='Building'>
  <MyComponent.State>
    <State variable='myVar1' dataType='String' default='Hello World'/>
    <State variable='myVar2' dataType='Boolean' default='true'/>
    <State variable='myVar3' dataType='Date'/>
  </MyComponent.State>
  <VerticalGroup>
    <Checkbox checked='=myVar2'/>
    <name visible='=myVar2'/>
  </VerticalGroup>
</MyComponent>
", result);
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
    public void NoUiComponentDefinition() {
      RunTest(@"
<Outer description='My description...' model='Building'>
</Outer>
", "UI Component Definition 'Outer' is empty (if it has any complex attributes, these don't count).  It will not be rendered as a visual component.", 2, 2);

      Assert.Single(_messages.Messages);
    }

    [Fact]
    public void ExpectingInstanceButDidNotGetOne() {
      RunTest(@"
<Outer description='My description...' model='Building'>
  <_a/>
  <name/>
</Outer>
", "Expecting either a Model Reference (e.g. <name\\>) or a Component Reference (e.g. <TextField path='name'\\>) or a Complex Attribute (e.g. SomeComponent.property) but got _a.", 3, 4);

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
    public void WrongComplexAttributeType() {
      RunTest(@"
<Outer description='My description...' model='Building' many='true'>
  <Table>
    <HelpIcon/>
    <Table.Header>
      <HelpIcon/>
    </Table.Header>
  </Table>
</Outer>
",
      "Complex Attribute value must be of type TableColumn or inherit from it",
      4, 6);

      Assert.Single(_messages.Messages);
    }

    [Fact]
    public void BadComplexAttribute() {
      RunTest(@"
<Outer description='My description...' model='Building' many='true'>
  <Table>
    <name/>
    <Table.Bogus>
    </Table.Bogus>
    <Table.Header>
      <HelpIcon/>
    </Table.Header>
  </Table>
</Outer>
", "Complex Attribute 'Bogus' does not exist on Component 'Table'", 5, 6);

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

    [Fact]
    public void UnknownAttributeInClassDef() {
      RunTest(@"
<Outer description='My description...' model='Building' foo='Junk'>
  <VerticalGroup>
  </VerticalGroup>
</Outer>
",
      "Unknown attribute 'foo' on Class Definition 'Outer'", 2, 57);

      Assert.Single(_messages.Messages);
    }

    [Fact]
    public void ChildContentInComponentWithNoPrimaryAttribute() {
      RunTest(@"
<Outer description='My description...'>
  <TextEdit>
    <HelpIcon/>
  </TextEdit>
</Outer>
",
      "Class Definition 'TextEdit' does not define a Primary Attribute, yet has child elements.", 3, 4);

      Assert.Single(_messages.Messages);
    }

    #endregion
    #endregion

    #region Pass 2.2 - Path Resolution, Model Ref UI Resolution, Attribute Reading

    #region Success
    [Fact]
    public void PathResolution() {
      ClassDefX10 definition = RunTest(@"
<MyComponent model='Building'>
  <VerticalGroup>       <!-- No path -->
    <name/>             <!-- Path for a Model Reference -->
    <Table path='demoApartment.rooms'>   <!-- double member path-->
      <Table.Header>
        <HelpIcon/>
      </Table.Header>
      <name/>
      <TableColumn label='Paint'>
        <Button path='paintColor' label='Boo' action='doSomething'/>    <!-- Many to single -->
      </TableColumn>
    </Table>
  </VerticalGroup>
</MyComponent>
");

      Assert.Empty(_messages.Messages);
      string result = Print(definition);

      Assert.Equal(@"<MyComponent model='Building'>
  <VerticalGroup>
    <name/>
    <Table path='demoApartment.rooms'>
      <Table.Header>
        <HelpIcon/>
      </Table.Header>
      <TableColumn>
        <name/>
      </TableColumn>
      <TableColumn label='Paint'>
        <Button path='paintColor' label='Boo' action='doSomething'/>
      </TableColumn>
    </Table>
  </VerticalGroup>
</MyComponent>
", result);
    }

    [Fact]
    public void EnumAttributeReading() {
      ClassDefX10 definition = RunTest(@"
<MyComponent model='Building'>
  <name weight='bold'/>
</MyComponent>
");

      Assert.Empty(_messages.Messages);
      string result = Print(definition);

      Assert.Equal(@"<MyComponent model='Building'>
  <name weight='bold'/>
</MyComponent>
", result);
    }

    [Fact]
    public void ResolveDerivedEntityAttributes() {
      ClassDefX10 definition = RunTest(@"
<MyComponent model='FancyRoom'>
  <VerticalGroup>
    <name/>
    <fancinessQuotient/>
  </VerticalGroup>
</MyComponent>
");

      Assert.Empty(_messages.Messages);
      string result = Print(definition);

      Assert.Equal(@"<MyComponent model='FancyRoom'>
  <VerticalGroup>
    <name/>
    <fancinessQuotient/>
  </VerticalGroup>
</MyComponent>
", result);
    }

    [Fact]
    public void ReadContextData() {
      ClassDefX10 definition = RunTest(@"
<MyComponent model='Building'>
  <TextEdit path='/currentUser.firstName'/>
</MyComponent>
");

      Assert.Empty(_messages.Messages);
      string result = Print(definition);

      Assert.Equal(@"<MyComponent model='Building'>
  <TextEdit path='/currentUser.firstName'/>
</MyComponent>
", result);
    }

    [Fact]
    public void ModelRefWithAttributes() {
      ClassDefX10 definition = RunTest(@"
<MyComponent model='Building'>
  <VerticalGroup>
    <hasUndergroundParking visible='=apartments.count > 10'/>
  </VerticalGroup>
</MyComponent>
");

      Assert.Empty(_messages.Messages);
      string result = Print(definition, new PrintConfig() { AlwaysPrintRenderAs = true });

      Assert.Equal(@"<MyComponent model='Building'>
  <VerticalGroup>
    <hasUndergroundParking visible='=apartments.count > 10' renderAs='Checkbox'/>
  </VerticalGroup>
</MyComponent>
", result);
    }
    #endregion

    #region Path-Related Tests

    [Fact]
    public void NestedModelReferencePath() {
      ClassDefX10 definition = RunTest(@"
<MyComponent model='Building'>
  <VerticalGroup>
    <demoApartment.number/>
  </VerticalGroup>
</MyComponent>
");

      Assert.Empty(_messages.Messages);
      string result = Print(definition);

      Assert.Equal(@"<MyComponent model='Building'>
  <VerticalGroup>
    <demoApartment.number/>
  </VerticalGroup>
</MyComponent>
", result);
    }

    [Fact]
    public void NonExistentPathMember() {
      RunTest(@"
<Outer model='Building'>
  <VerticalGroup path='bogus'>
    <Button path='doubleBogus' label='Boo' action='doSomething'/>   <!-- Does not get here -->
  </VerticalGroup>
</Outer>
", "Member 'bogus' does not exist on 'Building'.", 3, 18);

      Assert.Single(_messages.Messages);
    }

    [Fact]
    public void NonExistentNestedPathMember() {
      RunTest(@"
<Outer model='Building'>
  <VerticalGroup path='demoApartment.windows'/>
</Outer>
", "Member 'windows' does not exist on 'Apartment'.", 3, 18);

      Assert.Single(_messages.Messages);
    }

    [Fact]
    public void BadModelReference() {
      RunTest(@"
<Outer description='My description...' model='Building'>
  <Table path='apartments'>
    <Table.Header>
      <HelpIcon/>
    </Table.Header>
    <nonExistent/>
  </Table>
</Outer>
", "Member 'nonExistent' does not exist on 'Apartment'.", 7, 6);

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
      "Unknown attribute 'foo' on Class Definition 'TextEdit'",
      "Unknown attribute 'bar' on Class Definition 'HelpIcon'");

      Assert.Equal(2, _messages.Messages.Count);
    }

    [Fact]
    public void WrongAtomicAttributeType() {
      RunTest(@"
<Outer description='My description...' model='Building'>
  <HelpIcon difficulty='seven'/>
</Outer>
",
      "Error parsing attribute 'difficulty': could not parse a(n) Integer from 'seven'. Examples of valid data of this type: 1, 7, -8.",
      3, 13);

      Assert.Single(_messages.Messages);
    }
    #endregion

    #region ValidateDataModelCompatibility
    [Fact]
    public void WrongEntityType() {
      RunTest(@"
<Outer description='My description...' model='Building'>
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
<Outer model='Room' many='true'>
  <RoomViewer3D/>
</Outer>
", "The component RoomViewer3D expects a SINGLE Entity, but the path is delivering MANY 'Room' Entities", 3, 4);

      Assert.Single(_messages.Messages);
    }

    [Fact]
    public void ModelAttributeOnManyEntities() {
      RunTest(@"
<Outer model='Building' many='true'>
  <MyFunkyIntComponent path='apartmentCount'/>
</Outer>
", "Attempt to access member 'apartmentCount' in context of Many<Building>", 3, 24);

      Assert.Single(_messages.Messages);
    }

    [Fact]
    public void WrongAtomicDataType() {
      RunTest(@"
<Outer model='Building'>
  <MyFunkyIntComponent path='name'/>
</Outer>
", "The component MyFunkyIntComponent expects Integer, but the path is delivering String", 3, 4);

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

    [Fact]
    public void ModelRefForAssociationWithNoUi() {
      RunTest(@"
<Outer description='My description...' model='Building'>
  <apartments/>
</Outer>
",
      "Could not identify UI Component for Association 'apartments' of Entity 'Building'",
      3, 4);

      Assert.Single(_messages.Messages);
    }
    #endregion

    #region Attached Attributes

    [Fact]
    public void AttachedAttributeSuccess() {
      ClassDefX10 definition = RunTest(@"
<Outer description='My description...' model='Building'>
  <WithAttached>
    <apartmentCount WithAttached.myAttached='1'/>
  </WithAttached>
</Outer>
");

      Assert.Empty(_messages.Messages);

      string result = Print(definition);
      Assert.Equal(@"<Outer description='My description...' model='Building'>
  <WithAttached>
    <apartmentCount WithAttached.myAttached='1'/>
  </WithAttached>
</Outer>
", result);
    }

    [Fact]
    public void AttachedAttributeSuccessBadEntity() {
      RunTest(@"
<Outer description='My description...' model='Building'>
  <WithAttached>
    <apartmentCount BadEntity.myAttached='1'/>
  </WithAttached>
</Outer>

",
      "UI Component 'BadEntity' not found",
      4, 21);

      Assert.Single(_messages.Messages);
    }

    [Fact]
    public void AttachedAttributeSuccessBadAttribute() {
      RunTest(@"
<Outer description='My description...' model='Building'>
  <WithAttached>
    <apartmentCount WithAttached.badAttribute='1'/>
  </WithAttached>
</Outer>

",
      "Atomic attribute 'badAttribute' not found on Class Definition 'WithAttached'",
      4, 21);

      Assert.Single(_messages.Messages);
    }
    #endregion

    #region Binding Complex Attribute to State
    [Fact]
    public void BindTableSelectdToStateVar() {
      ClassDefX10 definition = RunTest(@"
<Buildings description='My description...' model='Building' many='true'>
  <Buildings.State>
    <State variable='selectedBuildings' model='Building' many='true'/>
  </Buildings.State>
  <Table selected='=selectedBuildings'>
    <Table.Header>
      <HelpIcon/>
    </Table.Header>
    <name/>
  </Table>
</Buildings>
");

      Assert.Empty(_messages.Messages);

      string result = Print(definition);
      Assert.Equal(@"<Buildings description='My description...' model='Building' many='True'>
  <Buildings.State>
    <State variable='selectedBuildings' model='Building' many='True'/>
  </Buildings.State>
  <Table selected='=selectedBuildings'>
    <Table.Header>
      <HelpIcon/>
    </Table.Header>
    <TableColumn>
      <name/>
    </TableColumn>
  </Table>
</Buildings>
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

      AllUiDefinitions allUiDefinitions = new AllUiDefinitions(_messages, null, new UiLibrary[] { _library });
      foreach (Entity entity in _allEntities.All)
        (new EntityCompilerPass3(allUiDefinitions)).CompileEntity(entity);

      CompilePass2(definition);

      return definition;
    }

    #endregion
  }
}
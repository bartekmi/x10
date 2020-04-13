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
  public class UiCompilerPass1Test : UiCompilerTestBase {

    public UiCompilerPass1Test(ITestOutputHelper output) : base(output) {
      // Do nothing
    }

    [Fact]
    public void CompileWithSetterAndNonSetter() {
      UiAttributeDefinitions.All.Add(new UiAttributeDefinitionAtomic() {
        Name = "customField",
        Description = "This is a custom field with no setter",
        AppliesTo = UiAppliesTo.UiDefinition,
        DataType = DataTypes.Singleton.String,
      });

      ClassDefX10 definition = RunTest(@"
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
        "Mandatory Atomic Attribute 'model' is missing", 2, 2);
    }

    [Fact]
    public void WarnIfNoChildrenAtRoot() {
      RunTest(@"
<MyComponent />
",
        "UI Component definition 'MyComponent' contains no children. It will not be rendered as a visual component",
        2, 2, CompileMessageSeverity.Warning);
    }

    [Fact]
    public void ErrorIfMultipleChildrenAtRoot() {
      RunTest(@"
<MyComponent>
  <child1/>
  <child2/>
</MyComponent>
",
        "UI Component definition 'MyComponent' has multiple children.",
        2, 2);
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
        "The name of the UI Component 'MyOtherComponent' must match the name of the file: MyComponent", 
        2, 2, CompileMessageSeverity.Error, "MyComponent.xml");
    }

    #region Utilities
    private ClassDefX10 RunTest(string xml, string fileName = null) {
      ClassDefX10 definition = TestUtils.UiCompilePass1(xml, _messages, _compilerPass1, _output, fileName);
      TestUtils.DumpMessages(_messages, _output);

      return definition;
    }

    private void RunTest(string xml, 
      string expectedErrorMessage, 
      int expectedLine, 
      int expectedChar, 
      CompileMessageSeverity expectedSeverity = CompileMessageSeverity.Error,
      string fileName = null) {

      RunTest(xml, fileName);

      CompileMessage message = _messages.Messages.FirstOrDefault(x => x.Message == expectedErrorMessage);
      Assert.NotNull(message);
      Assert.Equal(expectedSeverity, message.Severity);

      Assert.Equal(expectedLine, message.ParseElement.Start.LineNumber);
      Assert.Equal(expectedChar, message.ParseElement.Start.CharacterPosition);
    }
    #endregion
  }
}
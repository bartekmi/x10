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
        },
        new ClassDefNative() {
          Name = "Table",
        },
        new ClassDefNative() {
          Name = "MyFunkyIntComponent",
        },
      };

      _library = new UiLibrary(definitions) {
        Name = "Test Library",
      };
    }
    #endregion

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


    #endregion
  }
}
using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;
using Xunit.Abstractions;

using x10.model;
using x10.model.definition;
using x10.parsing;

namespace x10.compiler {
  public class FunctionsCompilerTest {

    private readonly ITestOutputHelper _output;
    private readonly MessageBucket _messages = new MessageBucket();
    private readonly FunctionsCompiler _compiler;

    public FunctionsCompilerTest(ITestOutputHelper output) {
      _output = output;
      AttributeReader attrReader = new AttributeReader(_messages);
      AllEntities allEntities = new AllEntities(_messages, new List<Entity>());
      _compiler = new FunctionsCompiler(_messages, allEntities, new AllEnums(_messages), new AllFunctions(_messages), attrReader);
    }

    [Fact]
    public void BadFunctionAttributeName() {
      RunTest(@"
name: MyFunc
description: My favorite function
returnDataType: Integer
arguments:
  - name: Attr1
    dataType: Integer
",
        "Invalid Function Argument name: 'Attr1'. Must be lower-cased camelCase: e.g. 'book', 'dateTime'. Numbers are also allowed.", 6, 11);
    }

    [Fact]
    public void AttributesMustBeListOfHashes() {
      RunTest(@"
name: MyFunc
description: My favorite function
returnDataType: Integer
arguments:
  - cat
  - dog
",
        "Expected a Hash type node, but was: TreeScalar", 6, 5);
    }

    [Fact]
    public void BadReturnDataType() {
      RunTest(@"
name: MyFunc
description: My favorite function
returnDataType: Bogus
",
        "Neither Enum nor a built-in data tyes 'Bogus' is defined", 4, 17);
    }

    [Fact]
    public void BadArgumentDataType() {
      RunTest(@"
name: MyFunc
description: My favorite function
returnDataType: Integer
arguments:
  - name: myArg
    dataType: Bogus
",
        "Data Type 'Bogus' of function argument 'myArg' was neither a built-in type, nor an enum, nor an Entity name", 7, 15);
    }

    [Fact]
    public void DuplicateArguments() {
      RunTest(@"
name: MyFunc
description: My favorite function
returnDataType: Integer
arguments:
  - name: myArgDup
    dataType: Integer
  - name: myArg
    dataType: Integer
  - name: myArgDup
    dataType: Integer
",
        "The name 'myArgDup' is not unique among all the arguments of this Function.", 6, 11);
    }

    #region Utilities

    private void RunTest(string yaml) {
      ParserYaml parser = new ParserYaml(_messages, null);
      TreeNode rootNode = parser.ParseFromString(yaml);
      Assert.NotNull(rootNode);

      _compiler.CompileFunction(rootNode);
      TestUtils.DumpMessages(_messages, _output);
    }

    private void RunTest(string yaml, string expectedErrorMessage, int expectedLine, int expectedChar) {
      RunTest(yaml);

      CompileMessage message = _messages.Messages.FirstOrDefault(x => x.Message == expectedErrorMessage);
      Assert.NotNull(message);

      Assert.Equal(expectedLine, message.ParseElement.Start.LineNumber);
      Assert.Equal(expectedChar, message.ParseElement.Start.CharacterPosition);
    }
    #endregion
  }
}
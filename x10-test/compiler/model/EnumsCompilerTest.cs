using System;
using System.Linq;

using Xunit;
using Xunit.Abstractions;

using x10.model;
using x10.parsing;

namespace x10.compiler {
  public class EnumsCompilerTest {

    private readonly ITestOutputHelper _output;
    private readonly MessageBucket _messages = new MessageBucket();
    private readonly EnumsCompiler _compiler;

    public EnumsCompilerTest(ITestOutputHelper output) {
      _output = output;
      AttributeReader attrReader = new AttributeReader(_messages);
      _compiler = new EnumsCompiler(_messages, new AllEnums(_messages), attrReader);
    }

    [Fact]
    public void BadDefault() {
      RunTest(@"
name: Animal
description: My favorit animals
values: monkey, cat, dog
default: banana
",
        "The default value 'banana' is not one of the available enum values", 5, 10);
    }

    [Fact]
    public void GoodDefault() {
      RunTest(@"
name: Animal
description: My favorit animals
values: monkey, cat, dog
default: cat
");
      Assert.Equal(0, _messages.Count);
    }

    [Fact]
    public void BadEnumValueName() {
      RunTest(@"
name: Animal
description: My favorit animals
values: monkey, cat, Dog
",
        "Invalid Enum value: 'Dog'. Must be lower-cased camelCase or ALL_CAPS: e.g. 'male', 'awaitingApproval, ASAP'. Numbers are also allowed.", 4, 9);
    }

    [Fact]
    public void EnumAsYamlListOfString() {
      RunTest(@"
name: Animal
description: My favorite animals
default: dog
values:
  - cat
  - dog
",
        "Expected a Hash type node, but was: TreeScalar", 6, 5);
    }

    #region Utilities

    private void RunTest(string yaml) {
      ParserYaml parser = new ParserYaml(_messages, null);
      TreeNode rootNode = parser.ParseFromString(yaml);
      Assert.NotNull(rootNode);

      _compiler.CompileEnum(rootNode, false);
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
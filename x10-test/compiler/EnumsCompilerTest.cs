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
  public class EnumsCompilerTest {

    private readonly ITestOutputHelper _output;
    private readonly MessageBucket _messages = new MessageBucket();
    private readonly EnumsCompiler _compiler;

    public EnumsCompilerTest(ITestOutputHelper output) {
      _output = output;
      AttributeReader attrReader = new AttributeReader(_messages);
      _compiler = new EnumsCompiler(_messages, attrReader);
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

    #region Utilities

    private void RunTest(string yaml) {
      ParserYaml parser = new ParserYaml();
      TreeNode rootNode = parser.ParseFromString(yaml);
      Assert.NotNull(rootNode);

      _compiler.CompileEnum(rootNode);
      TestUtils.DumpMessages(_messages, _output);
    }

    private void RunTest(string yaml, string expectedErrorMessage, int expectedLine, int expectedChar) {
      RunTest(yaml);

      CompileMessage message = _messages.Messages.FirstOrDefault(x => x.Message == expectedErrorMessage);
      Assert.NotNull(message);

      Assert.Equal(expectedLine, message.TreeElement.Start.LineNumber);
      Assert.Equal(expectedChar, message.TreeElement.Start.CharacterPosition);
    }
    #endregion
  }
}
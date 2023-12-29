using Xunit;
using Xunit.Abstractions;

using x10.parsing;
using x10.gen.typescript.library;

namespace x10.gen.react {
  public class ValidateChakraLibrary {

    private readonly ITestOutputHelper _output;
    private readonly MessageBucket _messages = new MessageBucket();

    public ValidateChakraLibrary(ITestOutputHelper output) {
      _output = output;
    }

    [Fact]
    public void Validate() {
      ChakraUI_Library.Singleton().HydrateAndValidate(_messages);

      TestUtils.DumpMessages(_messages, _output);
      Assert.True(_messages.IsEmpty);
    }
  }
}
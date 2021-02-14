using Xunit;
using Xunit.Abstractions;

using x10.parsing;
using x10.gen.react.library;

namespace x10.gen.react {
  public class ValidateReactLibrary {

    private readonly ITestOutputHelper _output;
    private readonly MessageBucket _messages = new MessageBucket();

    public ValidateReactLibrary(ITestOutputHelper output) {
      _output = output;
    }

    [Fact]
    public void Validate() {
      LatitudeLibrary.Singleton().HydrateAndValidate(_messages);

      TestUtils.DumpMessages(_messages, _output);
      Assert.True(_messages.IsEmpty);
    }
  }
}
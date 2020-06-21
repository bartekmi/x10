using x10.parsing;
using x10.ui.libraries;
using x10.ui.platform;

using Xunit;
using Xunit.Abstractions;

namespace x10.gen.wpf {
  public class ValidateWpfLibrary {

    private readonly ITestOutputHelper _output;
    private readonly MessageBucket _messages = new MessageBucket();

    public ValidateWpfLibrary(ITestOutputHelper output) {
      _output = output;
    }

    [Fact]
    public void Validate() {
      PlatformLibraryValidator validator = new PlatformLibraryValidator(_messages);
      validator.HydrateAndValidate(BaseLibrary.Singleton(), WpfBaseLibrary.Singleton());

      TestUtils.DumpMessages(_messages, _output);
      Assert.True(_messages.IsEmpty);
    }
  }
}
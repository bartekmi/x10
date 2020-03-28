using System;
using System.Collections.Generic;
using System.Text;

using Xunit.Abstractions;

using x10.parsing;

namespace x10 {
  public class TestUtils {
    public static void DumpMessages(MessageBucket messages, ITestOutputHelper output) {
      if (messages.IsEmpty)
        output.WriteLine("No Errors");
      else
        foreach (CompileMessage message in messages.Messages)
          output.WriteLine(message.ToString());
    }
  }
}

using System;
using System.Collections.Generic;
using System.Text;

using Xunit.Abstractions;

using x10.parsing;

using Xunit;
[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly)]

namespace x10 {
  public class TestUtils {
    public static void DumpMessages(MessageBucket messages, ITestOutputHelper output, CompileMessageSeverity? severities = null) {
      if (messages.IsEmpty)
        output.WriteLine("No Errors");
      else
        foreach (CompileMessage message in messages.FilteredMessages(severities))
          output.WriteLine(message.ToString());
    }
  }
}

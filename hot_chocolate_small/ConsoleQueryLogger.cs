using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using HotChocolate.Execution;
using HotChocolate.Execution.Instrumentation;
using Microsoft.Extensions.Logging;

namespace x10.hotchoc {
  public class ConsoleQueryLogger : ExecutionDiagnosticEventListener {
    private readonly ILogger<ConsoleQueryLogger> _logger;

    public ConsoleQueryLogger(ILogger<ConsoleQueryLogger> logger)
        => _logger = logger;

    public override IDisposable ExecuteRequest(IRequestContext context) {
      Console.WriteLine("Console ExecuteRequest: " + context);
      return EmptyScope;
    }

    public override void RequestError(IRequestContext context, Exception exception) {
      Console.WriteLine("Console RequestError: " + context);
      _logger.LogError(exception, "A request error occurred!");
    }  
  }
}
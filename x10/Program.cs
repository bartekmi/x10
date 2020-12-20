using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using x10.parsing;
using x10.compiler;
using x10.model;
using x10.ui.platform;
using x10.ui.libraries;
using x10.ui.metadata;
using x10.gen.react;

[assembly: InternalsVisibleTo("x10-test")]

namespace x10 {
  public class Program {
    public static void Main(string[] args) {

      MessageBucket _messages = new MessageBucket();

      string sourceDir = "examples/small";
      CompileEverything(_messages, sourceDir,
        out AllEntities allEntities,
        out AllEnums allEnums,
        out AllFunctions allFuncs,
        out AllUiDefinitions allUiDefinitions);

      PlatformLibrary[] libraries = new PlatformLibrary[] {
        LatitudeLibrary.Singleton(_messages, BaseLibrary.Singleton()),
      };

      DumpMessages(_messages);
      if (_messages.Errors.Count() > 0 )
        throw new Exception("Errors during compilation");

      string targetDir = "../react_generated_small/__generated__";
      ReactCodeGenerator generator = new ReactCodeGenerator(
        _messages,
        targetDir,
        allEntities,
        allEnums,
        allUiDefinitions,
        libraries);

      _messages.Clear();
      generator.Generate();

      DumpMessages(_messages);
    }

    internal static void CompileEverything(MessageBucket messages, string rootDir,
      out AllEntities allEntities, out AllEnums allEnums, out AllFunctions allFunctions, out AllUiDefinitions allUiDefinitions) {

      IEnumerable<UiLibrary> libraries = new UiLibrary[] {
        BaseLibrary.Singleton(),
        IconLibrary.Singleton(),
      };

      foreach (UiLibrary library in libraries)
        if (!library.HydrateAndValidate(messages)) {
          DumpMessages(messages);
          if (!messages.IsEmpty)
            throw new Exception("Problems with library: " + library.Name);
        }

      TopLevelCompiler compiler = new TopLevelCompiler(messages, libraries);
      compiler.Compile(rootDir, out allEntities, out allEnums, out allFunctions, out allUiDefinitions);

      DumpMessages(messages, CompileMessageSeverity.Error);
    }

    public static void DumpMessages(MessageBucket messages, CompileMessageSeverity? severities = null) {
      if (messages.IsEmpty)
        Console.WriteLine("No Errors");
      else
        foreach (CompileMessage message in messages.FilteredMessages(severities))
          Console.WriteLine(message.ToString());
    }
  }
}

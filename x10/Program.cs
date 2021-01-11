using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using x10.parsing;
using x10.compiler;
using x10.model;
using x10.ui.platform;
using x10.ui.libraries;
using x10.ui.metadata;

using x10.gen;
using x10.gen.react;
using x10.gen.wpf;

[assembly: InternalsVisibleTo("x10-test")]

namespace x10 {

  public class GenConfig {
    public string TargetDir {get;set;}
    public PlatformLibrary[] Libraries {get;set;}
    public CodeGenerator Generator {get;set;}
  }

  public class Program {
    private const string INTERMEDIATE_FILES_DIR = "temp/x10";

    private static MessageBucket _messages = new MessageBucket();

    private static readonly GenConfig REACT_SMALL_CONFIG = new GenConfig() {
      TargetDir = "../react_small_generated/x10_generated",
      Libraries = new PlatformLibrary[] {LatitudeLibrary.Singleton(_messages, BaseLibrary.Singleton())},
      Generator = new ReactCodeGenerator(),
    };

    private static readonly GenConfig WPF_SMALL_CONFIG = new GenConfig() {
      TargetDir = "../wpf_generated_small/__generated__",
      Libraries = new PlatformLibrary[] {WpfBaseLibrary.Singleton(_messages, BaseLibrary.Singleton())},
      Generator = new WpfCodeGenerator("wpf_generated"),
    };

    public static void Main(string[] args) {

      GenConfig config = REACT_SMALL_CONFIG;
      //GenConfig config = WPF_SMALL_CONFIG;
      
      MessageBucket messages = new MessageBucket();

      string sourceDir = "examples/small";
      CompileEverything(messages, sourceDir,
        out AllEntities allEntities,
        out AllEnums allEnums,
        out AllFunctions allFuncs,
        out AllUiDefinitions allUiDefinitions);

      PlatformLibrary[] libraries = new PlatformLibrary[] {
        LatitudeLibrary.Singleton(messages, BaseLibrary.Singleton()),
      };

      DumpMessages(messages);
      if (messages.Errors.Count() > 0 )
        throw new Exception("Errors during compilation");

      CodeGenerator generator = config.Generator;
      generator.IntermediateFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        INTERMEDIATE_FILES_DIR);

      messages.Clear();
      generator.Generate(
        messages, 
        config.TargetDir,
        allEntities,
        allEnums,
        allUiDefinitions,
        libraries);

      DumpMessages(messages);
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

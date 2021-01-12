using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Diagnostics;

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

  public class ScriptInfo {
    public string Script {get;set;}
    public string Args {get;set;}
  }

  public class GenConfig {
    public string ProjectDir { get; set; }
    public string TargetDir { get; set; }
    public PlatformLibrary[] Libraries { get; set; }
    public CodeGenerator Generator { get; set; }
    public ScriptInfo PostGenerationScript { get; set; }
  }

  public class Program {
    private const string INTERMEDIATE_FILES_DIR = "temp/x10";

    private static MessageBucket _messages = new MessageBucket();

    private static readonly GenConfig REACT_SMALL_CONFIG = new GenConfig() {
      ProjectDir = "../react_small_generated",
      TargetDir = "x10_generated",
      Libraries = new PlatformLibrary[] { LatitudeLibrary.Singleton(_messages, BaseLibrary.Singleton()) },
      Generator = new ReactCodeGenerator(),
      PostGenerationScript = new ScriptInfo() {
        Script = "yarn",
        Args = "relay",
      }
    };

    private static readonly GenConfig WPF_SMALL_CONFIG = new GenConfig() {
      ProjectDir = "../wpf_generated_small",
      TargetDir = "__generated__",
      Libraries = new PlatformLibrary[] { WpfBaseLibrary.Singleton(_messages, BaseLibrary.Singleton()) },
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
      if (messages.Errors.Count() > 0)
        throw new Exception("Errors during compilation");

      CodeGenerator generator = config.Generator;
      generator.IntermediateFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        INTERMEDIATE_FILES_DIR);

      messages.Clear();
      generator.Generate(
        messages,
        Path.Combine(config.ProjectDir, config.TargetDir),
        allEntities,
        allEnums,
        allUiDefinitions,
        libraries);

      DumpMessages(messages);

      ExecutePostBuildScript(config);
    }

    private static void ExecutePostBuildScript(GenConfig config) {
      ScriptInfo scriptInfo = config.PostGenerationScript;
      if (scriptInfo == null) return;

      ProcessStartInfo startInfo = new ProcessStartInfo();

      startInfo.WorkingDirectory = config.ProjectDir;
      startInfo.FileName = scriptInfo.Script;
      startInfo.Arguments = scriptInfo.Args;
      startInfo.RedirectStandardOutput = true;
      startInfo.RedirectStandardError = true;

      Console.WriteLine(string.Format("Executing: {0} {1}", scriptInfo.Script, scriptInfo.Args));
      Process process = Process.Start(startInfo);

      ReadAndDumpStream(process.StandardOutput);
      ReadAndDumpStream(process.StandardError);

      process.WaitForExit();
    }

    private static void ReadAndDumpStream(StreamReader reader) {
      string output = reader.ReadToEnd();
      Console.WriteLine(output);
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

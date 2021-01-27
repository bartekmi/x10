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
using x10.gen.hotchoc;

[assembly: InternalsVisibleTo("x10-test")]

namespace x10 {

  public class ScriptInfo {
    public string Script { get; set; }
    public string Args { get; set; }
  }

  public class GenConfig {
    public string SourceDir { get; set; }
    public string ProjectDir { get; set; }
    public string TargetDir { get; set; }
    public PlatformLibrary[] PlatformLibraries { get; set; }
    public UiLibrary[] LogicalLibraries { get; set; }
    public CodeGenerator Generator { get; set; }
    public ScriptInfo PostGenerationScript { get; set; }
  }

  public class Program {
    private const string INTERMEDIATE_FILES_DIR = "temp/x10";

    private static readonly GenConfig REACT_SMALL_CONFIG = new GenConfig() {
      SourceDir = "examples/small",
      ProjectDir = "../react_small_generated",
      TargetDir = "x10_generated",
      LogicalLibraries = new UiLibrary[] { BaseLibrary.Singleton(), IconLibrary.Singleton() },
      PlatformLibraries = new PlatformLibrary[] { LatitudeLibrary.Singleton() },
      Generator = new ReactCodeGenerator(),
      PostGenerationScript = new ScriptInfo() {
        Script = "yarn",
        Args = "relay",
      }
    };

    private static readonly GenConfig HOTCHOC_SMALL_CONFIG = new GenConfig() {
      SourceDir = "examples/small",
      ProjectDir = "../hot_chocolate_small",
      TargetDir = "SmallSample",
      LogicalLibraries = new UiLibrary[] { BaseLibrary.Singleton(), IconLibrary.Singleton() },
      PlatformLibraries = new PlatformLibrary[] { LatitudeLibrary.Singleton() },
      Generator = new HotchocCodeGenerator() {
        GenerateAbstractEntities = true,
      },
    };

    //  Outdated
    // private static readonly GenConfig WPF_SMALL_CONFIG = new GenConfig() {
    //   ProjectDir = "../wpf_generated_small",
    //   TargetDir = "__generated__",
    //   PlatformLibraries = new PlatformLibrary[] { WpfBaseLibrary.Singleton() },
    //   Generator = new WpfCodeGenerator("wpf_generated"),
    // };

    public static int Main(string[] args) {

      GenConfig config = HOTCHOC_SMALL_CONFIG;
      MessageBucket messages = new MessageBucket();

      // Hydrate UiLibraries
      // TODO: Ideally, this is not needed... Each platform library should know
      // its dependencies and hydrate them.
      foreach (UiLibrary library in config.LogicalLibraries) {
        if (!library.HydrateAndValidate(messages)) {
          DumpMessages("Logical Library Validation for " + library.Name, messages);
          return 1;
        }
      }

      // Hydrate Platform Libraries
      messages.Clear();
      foreach (PlatformLibrary library in config.PlatformLibraries) {
        if (!library.HydrateAndValidate(messages)) {
          DumpMessages("Platform Library Validation for " + library.Name, messages);
          return 1;
        }
      }

      // Compile
      messages.Clear();
      CompileEverything(messages, config,
        out AllEntities allEntities,
        out AllEnums allEnums,
        out AllFunctions allFuncs,
        out AllUiDefinitions allUiDefinitions);

      DumpMessages("Compilation", messages, CompileMessageSeverity.Error);
      if (messages.Errors.Count() > 0)
        return 1;

      // Generate Code
      messages.Clear();
      CodeGenerator generator = config.Generator;
      generator.IntermediateFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        INTERMEDIATE_FILES_DIR);

      generator.Generate(
        messages,
        Path.Combine(config.ProjectDir, config.TargetDir),
        allEntities,
        allEnums,
        allUiDefinitions,
        config.PlatformLibraries);

      DumpMessages("Code-Generation", messages);

      // Execute Post-Build
      ExecutePostBuildScript(config);

      return 0;
    }

    internal static void CompileEverything(MessageBucket messages, GenConfig config,
      out AllEntities allEntities, out AllEnums allEnums, out AllFunctions allFunctions, out AllUiDefinitions allUiDefinitions) {

      TopLevelCompiler compiler = new TopLevelCompiler(messages, config.LogicalLibraries);
      compiler.Compile(config.SourceDir, out allEntities, out allEnums, out allFunctions, out allUiDefinitions);
    }

    #region Post-Build Script
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
    #endregion

    #region Utils
    public static void DumpMessages(string label, MessageBucket messages, CompileMessageSeverity? severities = null) {
      Console.WriteLine("Messages for: " + label);
      if (messages.IsEmpty)
        Console.WriteLine("No Errors");
      else
        foreach (CompileMessage message in messages.FilteredMessages(severities))
          Console.WriteLine(message.ToString());

      Console.WriteLine();
    }
    #endregion
  }
}

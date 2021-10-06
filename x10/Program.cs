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
using x10.gen.react.library;
using x10.gen.react.generate;
using x10.gen.hotchoc;

[assembly: InternalsVisibleTo("x10-test")]

namespace x10 {

  public class ScriptInfo {
    public string Script { get; set; }
    public string Args { get; set; }
  }

  public class GenConfig {
    public string Name { get; set; }
    public string CommandLine { get; set; }
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

    private static readonly GenConfig[] CONFIGS = new GenConfig[] {
      new GenConfig() {
        Name = "Small Project - React",
        CommandLine = "smallreact",
        SourceDir = "examples/small",
        ProjectDir = "../react_small_generated",
        TargetDir = "x10_generated/small",
        LogicalLibraries = new UiLibrary[] { BaseLibrary.Singleton(), IconLibrary.Singleton() },
        PlatformLibraries = new PlatformLibrary[] { LatitudeLibrary.Singleton() },
        Generator = new ReactCodeGenerator() {
          GeneratedCodeSubdir = "small",
          AppContextImport = "SmallAppContext",
        },
        PostGenerationScript = new ScriptInfo() {
          Script = "yarn",
          Args = "relay-small",
        }
      },

      new GenConfig() {
        Name = "DPS - React",
        CommandLine = "dps",
        SourceDir = "examples/dps",
        ProjectDir = "../react_small_generated",
        TargetDir = "x10_generated/dps",
        LogicalLibraries = new UiLibrary[] { BaseLibrary.Singleton(), IconLibrary.Singleton() },
        PlatformLibraries = new PlatformLibrary[] { LatitudeLibrary.Singleton() },
        Generator = new ReactCodeGenerator() {
          GeneratedCodeSubdir = "dps",
        },
        PostGenerationScript = new ScriptInfo() {
          Script = "yarn",
          Args = "relay-dps",
        }
      },

      new GenConfig() {
        Name = "Client Page Project - React",
        CommandLine = "cpreact",
        SourceDir = "examples/client_page",
        ProjectDir = "../react_small_generated",
        TargetDir = "x10_generated/client_page",
        LogicalLibraries = new UiLibrary[] { BaseLibrary.Singleton(), IconLibrary.Singleton(), FlexportSpecialLibrary.Singleton() },
        PlatformLibraries = new PlatformLibrary[] { LatitudeLibrary.Singleton(), LatitudeFlexportSpecialLibrary.Singleton() },
        Generator = new ReactCodeGenerator() {
          GeneratedCodeSubdir = "client_page",
          AppContextImport = "ClientPageAppContext",
        },
        PostGenerationScript = new ScriptInfo() {
          Script = "yarn",
          Args = "relay-cp",
        }
      },

      new GenConfig() {
        Name = "Small Project - Hot Chocolate",
        CommandLine = "smallhot",
        SourceDir = "examples/small",
        ProjectDir = "../hot_chocolate_small",
        TargetDir = "SmallSample",
        LogicalLibraries = new UiLibrary[] { BaseLibrary.Singleton(), IconLibrary.Singleton() },
        PlatformLibraries = new PlatformLibrary[] { LatitudeLibrary.Singleton() },
        Generator = new HotchocCodeGenerator() {
          GenerateAbstractEntities = true,
          PackageName = "SmallSample",
        },
      },

      new GenConfig() {
        Name = "DPS - Hot Chocolate",
        CommandLine = "dpshot",
        SourceDir = "examples/dps",
        ProjectDir = "../hot_chocolate_small",
        TargetDir = "dps",
        LogicalLibraries = new UiLibrary[] { BaseLibrary.Singleton(), IconLibrary.Singleton() },
        PlatformLibraries = new PlatformLibrary[] { LatitudeLibrary.Singleton() },
        Generator = new HotchocCodeGenerator() {
          GenerateAbstractEntities = true,
          PackageName = "dps",
        },
      },

      new GenConfig() {
        Name = "Client Page Project - Hot Chocolate",
        CommandLine = "cphot",
        SourceDir = "examples/client_page",
        ProjectDir = "../hot_chocolate_small",
        TargetDir = "ClientPage",
        LogicalLibraries = new UiLibrary[] { BaseLibrary.Singleton(), IconLibrary.Singleton(), FlexportSpecialLibrary.Singleton() },
        PlatformLibraries = new PlatformLibrary[] { },
        Generator = new HotchocCodeGenerator() {
          GenerateAbstractEntities = true,
          PackageName = "ClientPage",
        },
      },
    };

    public static int Main(string[] args) {
      IEnumerable<GenConfig> configs = ExtractConfigs(args);

      foreach (GenConfig config in configs) {
        Console.WriteLine("********************************************* Generating and Compiling: " + config.Name);
        CompileAndGenerate(config);
      }

      return 0;
    }

    private static void CompileAndGenerate(GenConfig config) {
      HydrateUiLibraries(config.LogicalLibraries);
      HydratePlatformLibraries(config.PlatformLibraries);

      // Compile
      CompileEverything(config,
        out AllEntities allEntities,
        out AllEnums allEnums,
        out AllFunctions allFuncs,
        out AllUiDefinitions allUiDefinitions);

      // Generate Code
      GenerateCode(config, allEntities, allEnums, allUiDefinitions);

      // Execute Post-Build
      ExecutePostBuildScript(config);

      // Generate docs for all logical libraries
      GenerateLibraryDocs(config);
    }

    private static void GenerateCode(GenConfig config, AllEntities allEntities, AllEnums allEnums, AllUiDefinitions allUiDefinitions) {
      MessageBucket messages = new MessageBucket();
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
    }

    private static void HydrateUiLibraries(IEnumerable<UiLibrary> libraries) {
      MessageBucket messages = new MessageBucket();
      foreach (UiLibrary library in libraries)
        if (!library.HydrateAndValidate(messages)) {
          DumpMessages("Logical Library Validation for " + library.Name, messages);
          Environment.Exit(1);
        }
    }

    private static void HydratePlatformLibraries(IEnumerable<PlatformLibrary> libraries) {
      MessageBucket messages = new MessageBucket();
      foreach (PlatformLibrary library in libraries)
        if (!library.HydrateAndValidate(messages)) {
          DumpMessages("Platform Library Validation for " + library.Name, messages);
          Environment.Exit(1);
        }
    }

    internal static void CompileEverything(GenConfig config,
      out AllEntities allEntities, out AllEnums allEnums, out AllFunctions allFunctions, out AllUiDefinitions allUiDefinitions) {

      MessageBucket messages = new MessageBucket();

      TopLevelCompiler compiler = new TopLevelCompiler(messages, config.LogicalLibraries);
      compiler.Compile(config.SourceDir, out allEntities, out allEnums, out allFunctions, out allUiDefinitions);

      if (messages.Errors.Count() > 0) {
        DumpMessages("Compilation", messages, CompileMessageSeverity.Error);
        Environment.Exit(1);
      }
    }

    #region Generate Library Docs
    private const string DOC_DIR = "doc";
    private static void GenerateLibraryDocs(GenConfig config) {
      if (!Directory.Exists(DOC_DIR))
        Directory.CreateDirectory(DOC_DIR);

      foreach (UiLibrary library in config.LogicalLibraries) {
        string filename = String.Format("{0}/{1}.md", 
          DOC_DIR,
          library.Name.Replace(" ", "_").ToLower());

        using (TextWriter writer = new StreamWriter(filename))
          library.GenerateMarkdown(writer);
      }
    }
    #endregion

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

    private static IEnumerable<GenConfig> ExtractConfigs(string[] args) {
      if (args.Length == 0)
        PrintUsageAndExit();

      if (args.Length == 1 && args[0] == "all")
        return CONFIGS;

      List<GenConfig> configs = new List<GenConfig>();
      foreach (string arg in args) {
        GenConfig config = CONFIGS.SingleOrDefault(x => x.CommandLine == arg);
        if (config == null)
          PrintUsageAndExit();
        configs.Add(config);
      }

      return configs;
    }

    private static void PrintUsageAndExit() {
      Console.WriteLine("Usage: dotnet run -- <{0}>",
        string.Join(" | ", CONFIGS.Select(x => x.CommandLine)));
      Environment.Exit(1);
    }

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

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Execution.Configuration;

using x10.ui.metadata;
using x10.ui.libraries;

namespace x10.hotchoc {

  public class HotChocConfig {
    public string CommandLine { get; private set; }
    public string ProjectName { get; private set; }
    public string MetadataDir { get; private set; }
    public Type RepositoryInterface { get; private set; }
    public RepositoryBase Repository { get; private set; }

    public UiLibrary[]? LogicalLibraries { get; set; }
    public string? IntermediateOutputDir { get; internal set; }
    public Action? PostInitializeAction { get; internal set; }
    public Action? PreInitializeAction { get; internal set; }

    // Derived
    public string SchemaOutputFile => string.Format("../{0}.graphql", ProjectName);

    internal HotChocConfig(
     string commandLine,
     string projectName,
     string metadataDir,
     Type repositoryInterface,
     RepositoryBase repository
    ) {
      CommandLine = commandLine;
      ProjectName = projectName;
      MetadataDir = metadataDir;
      RepositoryInterface = repositoryInterface;
      Repository = repository;
    }
  }

  public class Program {

    private static readonly HotChocConfig[] CONFIGS = new HotChocConfig[] {
      new HotChocConfig(
        commandLine: "small",
        projectName: "SmallSample",
        metadataDir: "../x10/examples/small",
        repositoryInterface: typeof(SmallSample.Repositories.IRepository),
        repository: new SmallSample.Repositories.Repository()
      ) {
        IntermediateOutputDir = "/Users/bartek/temp/small",
      },
      new HotChocConfig(
        commandLine: "dps",
        projectName: "dps",
        metadataDir: "../x10/examples/dps",
        repositoryInterface: typeof(dps.Repositories.IRepository),
        repository: new dps.Repositories.Repository()
      ) {
        IntermediateOutputDir = "/Users/bartek/temp/dps",
        LogicalLibraries = new UiLibrary[] { BaseLibrary.Singleton(), IconLibrary.Singleton() },
      },
      new HotChocConfig(
        commandLine: "cp",
        projectName: "ClientPage",
        metadataDir: "../x10/examples/client_page",
        repositoryInterface: typeof(ClientPage.Repositories.IRepository),
        repository: new ClientPage.Repositories.Repository()
      ) {
        IntermediateOutputDir = "/Users/bartek/temp/client_page",
        PreInitializeAction = () => {
          // This is lame (Tech Debt), but there is the following dependency:
          // DataIngest uses EntityAndEnumCompiler (EEC), EEC loads Functions, Functions have a dependency on
          // a data-type defined in FlexportSpecialLibrary = BOOM!
          // The following lines force a load of the data-type defined in the UI libraries.
          //
          // One solution to this would be to pass in a flag to EEC to tell it NOT to compile functions under the
          // "ui" directory, but this instantly makes the "ui" directory "special" - the type of obscure rule
          // that future generations of developers would curse me for.      
          BaseLibrary.Singleton();
          FlexportSpecialLibrary.Singleton();
        },
      },
    };

    internal static HotChocConfig Config = null!;

    public static async Task Main(string[] args) {
      Config = ExtractConfig(args);

      if (Config.PreInitializeAction != null)
        Config.PreInitializeAction();

      DataIngest.GenerateTestData(Config);

      if (Config.PostInitializeAction != null)
        Config.PostInitializeAction();

      // https://hotchocolategraphql.slack.com/archives/CD9TNKT8T/p1604414586468700
      IRequestExecutorBuilder reBuilder = Startup.BuildSchema(new ServiceCollection());
      ISchema schema = await reBuilder.BuildSchemaAsync();
      File.WriteAllText(Config.SchemaOutputFile, schema.ToString());

      IHost host = CreateHostBuilder(args).Build();

      reBuilder.AddDiagnosticEventListener(sp =>
        new ConsoleQueryLogger(
          sp.GetApplicationService<ILogger<ConsoleQueryLogger>>()
        ));

      host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) {
      return Host.CreateDefaultBuilder(args)
          .ConfigureLogging(c => c.AddConsole())
          .ConfigureWebHostDefaults(webBuilder => {
            webBuilder.UseStartup<Startup>()
                      .UseUrls("http://localhost:5001");
          });
    }

    private static HotChocConfig ExtractConfig(string[] args) {
      if (args.Length != 1)
        PrintUsageAndExit();

      HotChocConfig? config = CONFIGS.SingleOrDefault(x => x.CommandLine == args[0]);
      if (config == null)
        PrintUsageAndExit();

      return config!;
    }

    private static void PrintUsageAndExit() {
      Console.WriteLine("Usage: dotnet run -- <{0}>",
        string.Join(" | ", CONFIGS.Select(x => x.CommandLine)));
      Environment.Exit(1);
    }
  }
}

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using HotChocolate;
using HotChocolate.Execution;

namespace x10.hotchoc {

  public class HotChocConfig {
    public string CommandLine { get; private set; }
    public string ProjectName { get; private set; }
    public string MetadataDir { get; private set; }
    public Type RepositoryInterface { get; private set; }
    public RepositoryBase Repository { get; private set; }

    public string? IntermediateOutputDir { get; internal set; }
    public Action? PostInitializeAction { get; internal set; }

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
        IntermediateOutputDir = "/Users/bmuszynski/temp/small",
      },
      new HotChocConfig(
        commandLine: "cp",
        projectName: "ClientPage",
        metadataDir: "../x10/examples/client_page",
        repositoryInterface: typeof(ClientPage.Repositories.IRepository),
        repository: new ClientPage.Repositories.Repository()
      ) {
        IntermediateOutputDir = "/Users/bmuszynski/temp/client_page",
        PostInitializeAction = () => {
          var repository = (x10.hotchoc.ClientPage.Repositories.Repository)Config.Repository;
          string json = JsonConvert.SerializeObject(repository.GetClients(), Formatting.Indented);
          Console.WriteLine(json);
        }
      },
    };

    internal static HotChocConfig Config = null!;

    public static async Task Main(string[] args) {
      Config = ExtractConfig(args);

      DataIngest.GenerateTestData(Config);
      if (Config.PostInitializeAction != null)
        Config.PostInitializeAction();

      // https://hotchocolategraphql.slack.com/archives/CD9TNKT8T/p1604414586468700
      ISchema schema = await Startup.BuildSchema(new ServiceCollection()).BuildSchemaAsync();
      File.WriteAllText(Config.SchemaOutputFile, schema.ToString());

      CreateHostBuilder(args)
        .Build()
        .Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());

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

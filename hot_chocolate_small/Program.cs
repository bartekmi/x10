using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using HotChocolate;
using HotChocolate.Execution;

namespace x10.hotchoc {

  public class HotChocConfig {
    public string CommandLine { get; set; }
    public string ProjectName { get; set; }
    public string MetadataDir { get; set; }
    public Type RepositoryInterface { get; set; }
    public RepositoryBase Repository {get;set;}

    // Derived
    public string SchemaOutputFile => string.Format("../{0}.graphql", ProjectName);
  }

  public class Program {

    private static readonly HotChocConfig[] CONFIGS = new HotChocConfig[] {
      new HotChocConfig() {
        CommandLine = "small",
        ProjectName = "SmallSample",
        MetadataDir = "../x10/examples/small",
        RepositoryInterface = typeof(SmallSample.Repositories.IRepository),
        Repository = new SmallSample.Repositories.Repository(),
      },
      new HotChocConfig() {
        CommandLine = "cp",
        ProjectName = "ClientPage",
        MetadataDir = "../x10/examples/client_page",
        RepositoryInterface = typeof(ClientPage.Repositories.IRepository),
        Repository = new ClientPage.Repositories.Repository(),
      },
    };

    internal static HotChocConfig Config;

    public static async Task Main(string[] args) {
      Config = ExtractConfig(args);

      DataIngest.GenerateTestData(Config);

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

      return config;
    }

    private static void PrintUsageAndExit() {
      Console.WriteLine("Usage: dotnet run -- <{0}>",
        string.Join(" | ", CONFIGS.Select(x => x.CommandLine)));
      Environment.Exit(1);
    }
  }
}

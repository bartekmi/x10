using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using HotChocolate;
using HotChocolate.Execution;


namespace x10.hotchoc {
  public class Program {
    // Use this as part of the path if necessary in future...
    // Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
    public const string SCHEMA_OUTPUT_FILE = "../schema.graphql";
    public const string X10_PROJECT_DIR = "../x10/examples/small";

    public static async Task Main(string[] args) {
      // https://hotchocolategraphql.slack.com/archives/CD9TNKT8T/p1604414586468700
      ISchema schema = await Startup.BuildSchema(new ServiceCollection()).BuildSchemaAsync();
      File.WriteAllText(SCHEMA_OUTPUT_FILE, schema.ToString());

      CreateHostBuilder(args)
        .Build()
        .Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
  }
}

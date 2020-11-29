using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Small {
  public class Program {
    // Use this as part of the path if necessary in future...
    // Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
    public const string SCHEMA_OUTPUT_FILE = "../react_small_hand_coded/schema.graphql";

    public static Task Main(string[] args) {
      return CreateHostBuilder(args).Build().RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
                webBuilder.UseStartup<Startup>());
  }
}

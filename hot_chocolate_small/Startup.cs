using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

using HotChocolate;
using HotChocolate.Execution.Configuration;

namespace x10.hotchoc {
  public class Startup {
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services) {
      services.AddSingleton(Program.Config.RepositoryInterface, Program.Config.Repository);

      services.AddCors();
      services.AddErrorFilter<MyErrorFilter>();

      BuildSchema(services)
        .AddApolloTracing();
    }

    internal static IRequestExecutorBuilder BuildSchema(IServiceCollection services) {
      IRequestExecutorBuilder builder = services
        .AddGraphQLServer()
        .AllowIntrospection(true)
        .AddQueryType(d => d.Name("Query"))
        .AddMutationType(d => d.Name("Mutation"))
        .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);

      foreach (Type type in Program.Config.Repository.Types())
        builder.AddType(type);

      return builder;
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      app
        .UseDeveloperExceptionPage()
        .UseCors(policy => {
          policy.AllowAnyHeader();
          policy.AllowAnyMethod();
          policy.SetIsOriginAllowed(origin => true); // allow any origin
          policy.AllowCredentials();
        })
        .UseRouting()
        .UseEndpoints(endpoints => {
          endpoints.MapGraphQL();
        });
    }
  }

  internal class MyErrorFilter : IErrorFilter {
      public IError OnError(IError error) {
          Console.WriteLine(error.Exception?.ToString() ?? error.Message);
          return error;
      }
  }
}

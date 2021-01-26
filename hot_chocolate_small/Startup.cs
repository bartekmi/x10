using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

using HotChocolate;
using HotChocolate.Execution.Configuration;

using x10.hotchoc.Entities;
using x10.hotchoc.Repositories;

namespace x10.hotchoc {
  public class Startup {
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services) {
      services.AddSingleton<ISmallRepository, SmallRepository>();

      services.AddCors();

      BuildSchema(services)
        .AddApolloTracing();
    }

    internal static IRequestExecutorBuilder BuildSchema(IServiceCollection services) {
      return services
        .AddGraphQLServer()

        // The two roots - queries and mutations
        .AddQueryType(d => d.Name("Query"))
        .AddMutationType(d => d.Name("Mutation"))
        .AddType<SmallQueries>()
        .AddType<SmallMutations>()

        // Application types
        .AddType<Address>()
        .AddType<Tenant>()
        .AddType<Unit>()
        .AddType<Building>()
        .AddType<Move>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      app
        .UseCors(policy => {
          policy.AllowAnyHeader();
          policy.AllowAnyMethod();
          policy.SetIsOriginAllowed(origin => true); // allow any origin
          policy.AllowCredentials();
        })        
        .UseRouting()
        .UseEndpoints(endpoints =>
        {
            endpoints.MapGraphQL();
        });
    }
  }
}

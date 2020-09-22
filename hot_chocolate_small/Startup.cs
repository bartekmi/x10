using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Voyager;
using HotChocolate.Subscriptions;

using Small;
using Small.Entities;
using Small.Repositories;

namespace Small {
  public class Startup {
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services) {
      // Add the custom services like repositories etc ...
      services.AddSingleton<ISmallRepository, SmallRepository>();

      // Add in-memory event provider
      services.AddInMemorySubscriptionProvider();

      // Add GraphQL Services
      services.AddGraphQL(sp => SchemaBuilder.New()
          .AddServices(sp)
          .AddQueryType(d => d.Name("Query"))
          .AddMutationType(d => d.Name("Mutation"))
          .AddSubscriptionType(d => d.Name("Subscription"))

          .AddType<SmallQueries>()
          .AddType<SmallMutations>()

          .AddType<Address>()
          .AddType<Tenant>()
          .AddType<Unit>()
          .AddType<Building>()
          .AddType<Move>()

          .Create());
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      }

      app
          .UseRouting()
          .UseWebSockets()
          .UseGraphQL("/graphql")
          .UsePlayground("/graphql")
          .UseVoyager("/graphql");
    }
  }
}

using GTT.Infrastructure;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

var hostBuilder = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(builder =>
    {
        builder.UseNewtonsoftJson(new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Include,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            }
        });
    });

var host = hostBuilder
    .ConfigureOpenApi()
    .ConfigureServices((context, services) =>
    {
        //services.AddOidcApiAuthorization();

        var configuration = context.Configuration;


        services
            .AddInfrastructure(configuration);

    })
    .Build();

await host.RunAsync();

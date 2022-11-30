using FluentValidation;
using GTT.Application;
using GTT.Application.Behaviors;
using GTT.Application.Services;
using GTT.Infrastructure.Data;
using GTT.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Thrive.Customers.Application.Queries;

namespace GTT.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddMediatR(services);
        AddServices(services);
        AddOptions(services);

        return services;
    }

    private static void AddMediatR(IServiceCollection services)
    {
        services.AddMediatR(typeof(GetClasses).Assembly);
        services.AddValidatorsFromAssembly(typeof(GetClasses).Assembly, includeInternalTypes: true);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddTransient(typeof(IDbConnectionFactory), typeof(SqlDbConnectionFactory));
        services.AddTransient(typeof(IChallengeService), typeof(ChallengeService));
    }
    private static void AddOptions(IServiceCollection services)
    {
        services.AddOptions<SqlOptions>()
            .Configure<IConfiguration>((options, configuration) =>
            {
                options.SqlConnectionString = configuration.GetValue<string>("SqlOptions:SqlConnectionString");
                options.SqlUseAccessToken = configuration.GetValue<bool>("SqlOptions:SqlUseAccessToken");
            });
    }
}

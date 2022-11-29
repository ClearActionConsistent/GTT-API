using FluentValidation;
using GTT.Application.Services;
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
        AddValidators(services);
        AddServices(services);
        AddOptions(services);

        return services;
    }

    

    private static void AddMediatR(IServiceCollection services)
    {
        //config mediatR pipeline here
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddMediatR(typeof(GetClasses).Assembly);
    }

    private static void AddValidators(IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(GetClasses).Assembly, includeInternalTypes: true);
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddTransient(typeof(IChallengeService), typeof(ChallengeService));
    }
    private static void AddOptions(IServiceCollection services)
    {

    }
}

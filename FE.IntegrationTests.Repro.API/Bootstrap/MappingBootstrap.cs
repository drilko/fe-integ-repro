using System.Reflection;

namespace FE.IntegrationTests.Repro.API.Bootstrap;

public static class MappingBootstrap
{
    public static IServiceCollection AddMapping(this IServiceCollection services)
    {
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        typeAdapterConfig.Scan(Assembly.GetExecutingAssembly());

        return services;
    }
}

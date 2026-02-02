using Microsoft.Extensions.DependencyInjection;
using RiskAnalisys.Application.Services;

namespace RiskAnalisys.Application.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddDIApplication(this IServiceCollection services)
    {
        services.AddScoped<IRiskAnalisysService, RiskAnalisysService>();

        return services;
    }
}

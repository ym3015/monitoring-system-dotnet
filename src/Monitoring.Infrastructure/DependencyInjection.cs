using Microsoft.Extensions.DependencyInjection;
using Monitoring.Application.Abstractions.Clock;
using Monitoring.Application.Abstractions.Network;
using Monitoring.Infrastructure.Clock;
using Monitoring.Infrastructure.Network;

namespace Monitoring.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IPingService, SystemPingService>();

        return services;
    }
}
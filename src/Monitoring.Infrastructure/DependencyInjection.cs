using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Monitoring.Application.Abstractions.Clock;
using Monitoring.Application.Abstractions.Network;
using Monitoring.Application.Abstractions.Persistence;
using Monitoring.Infrastructure.Clock;
using Monitoring.Infrastructure.Network;
using Monitoring.Infrastructure.Persistence;
using Monitoring.Infrastructure.Persistence.Repositories;

namespace Monitoring.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<MonitoringDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("MonitoringDatabase"));
        });

        services.AddScoped<IDeviceRepository, DeviceRepository>();
        services.AddScoped<IPingResultRepository, PingResultRepository>();

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IPingService, SystemPingService>();

        return services;
    }
}
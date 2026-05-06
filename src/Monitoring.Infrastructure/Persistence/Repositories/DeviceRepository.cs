using Microsoft.EntityFrameworkCore;
using Monitoring.Application.Abstractions.Persistence;
using Monitoring.Domain.Devices;

namespace Monitoring.Infrastructure.Persistence.Repositories;

public sealed class DeviceRepository : IDeviceRepository
{
    private readonly MonitoringDbContext _dbContext;

    public DeviceRepository(MonitoringDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Device>> GetEnabledDevicesAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Devices
                               .AsNoTracking()
                               .Where(x => x.IsEnabled)
                               .ToListAsync(cancellationToken);
    }
}
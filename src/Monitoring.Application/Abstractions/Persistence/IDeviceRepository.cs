using Monitoring.Domain.Devices;

namespace Monitoring.Application.Abstractions.Persistence;

public interface IDeviceRepository
{
    Task<IReadOnlyList<Device>> GetEnabledDevicesAsync(CancellationToken cancellationToken);
}
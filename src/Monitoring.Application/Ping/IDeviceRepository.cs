using Monitoring.Domain.Devices;

namespace Monitoring.Application.Devices;

public interface IDeviceRepository
{
    Task<IReadOnlyList<Device>> GetEnabledDevicesAsync(CancellationToken cancellationToken);
}
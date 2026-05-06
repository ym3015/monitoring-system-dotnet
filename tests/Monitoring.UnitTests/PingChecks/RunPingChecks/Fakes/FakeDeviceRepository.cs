using Monitoring.Application.Abstractions.Persistence;
using Monitoring.Domain.Devices;

namespace Monitoring.UnitTests.PingChecks.RunPingChecks.Fakes;

internal sealed class FakeDeviceRepository : IDeviceRepository
{
    private readonly IReadOnlyList<Device> _devices;

    public FakeDeviceRepository(IReadOnlyList<Device> devices)
    {
        _devices = devices;
    }

    public Task<IReadOnlyList<Device>> GetEnabledDevicesAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(_devices);
    }
}

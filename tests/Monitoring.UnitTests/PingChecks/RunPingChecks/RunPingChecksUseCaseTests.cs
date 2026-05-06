using Monitoring.Application.PingChecks.RunPingChecks;
using Monitoring.Domain.Devices;
using Monitoring.Domain.Ping;
using Monitoring.UnitTests.PingChecks.RunPingChecks.Fakes;

namespace Monitoring.UnitTests.PingChecks.RunPingChecks;

public sealed class RunPingChecksUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_WhenEnabledDevicesExist_PingsEachDeviceAndStoresResults()
    {
        var checkedAtUtc = new DateTime(2026, 5, 6, 10, 30, 0, DateTimeKind.Utc);
        var devices = new[]
        {
            new Device { Id = 1, Name = "Router", IpAddress = "192.168.1.1", IsEnabled = true },
            new Device { Id = 2, Name = "Switch", IpAddress = "192.168.1.2", IsEnabled = true }
        };

        var deviceRepository = new FakeDeviceRepository(devices);
        var pingService = new FakePingService(new Dictionary<string, PingCheckResult>
        {
            ["192.168.1.1"] = new("192.168.1.1", PingStatus.Success, 12, null),
            ["192.168.1.2"] = new("192.168.1.2", PingStatus.Failed, 0, "TimedOut")
        });
        var pingResultRepository = new FakePingResultRepository();
        var dateTimeProvider = new FakeDateTimeProvider(checkedAtUtc);

        var useCase = new RunPingChecksUseCase(
            deviceRepository,
            pingService,
            pingResultRepository,
            dateTimeProvider);

        await useCase.ExecuteAsync(CancellationToken.None);

        Assert.Equal(new[] { "192.168.1.1", "192.168.1.2" }, pingService.CheckedIpAddresses);
        Assert.Collection(
            pingResultRepository.Results,
            first =>
            {
                Assert.Equal(1, first.DeviceId);
                Assert.Equal("192.168.1.1", first.IpAddress);
                Assert.Equal(PingStatus.Success, first.Status);
                Assert.Equal(12, first.ResponseTimeMs);
                Assert.Null(first.ErrorMessage);
                Assert.Equal(checkedAtUtc, first.CheckedAtUtc);
            },
            second =>
            {
                Assert.Equal(2, second.DeviceId);
                Assert.Equal("192.168.1.2", second.IpAddress);
                Assert.Equal(PingStatus.Failed, second.Status);
                Assert.Equal(0, second.ResponseTimeMs);
                Assert.Equal("TimedOut", second.ErrorMessage);
                Assert.Equal(checkedAtUtc, second.CheckedAtUtc);
            });
    }

    [Fact]
    public async Task ExecuteAsync_WhenNoEnabledDevicesExist_DoesNotPingOrStoreResults()
    {
        var deviceRepository = new FakeDeviceRepository(Array.Empty<Device>());
        var pingService = new FakePingService(new Dictionary<string, PingCheckResult>());
        var pingResultRepository = new FakePingResultRepository();
        var dateTimeProvider = new FakeDateTimeProvider(DateTime.UtcNow);

        var useCase = new RunPingChecksUseCase(
            deviceRepository,
            pingService,
            pingResultRepository,
            dateTimeProvider);

        await useCase.ExecuteAsync(CancellationToken.None);

        Assert.Empty(pingService.CheckedIpAddresses);
        Assert.Empty(pingResultRepository.Results);
    }
}

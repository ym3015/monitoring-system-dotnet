using Monitoring.Application.Abstractions.Clock;
using Monitoring.Application.Abstractions.Network;
using Monitoring.Application.Abstractions.Persistence;
using Monitoring.Domain.Ping;

namespace Monitoring.Application.PingChecks.RunPingChecks;

public sealed class RunPingChecksUseCase
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IPingService _pingService;
    private readonly IPingResultRepository _pingResultRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public RunPingChecksUseCase(
        IDeviceRepository deviceRepository,
        IPingService pingService,
        IPingResultRepository pingResultRepository,
        IDateTimeProvider dateTimeProvider)
    {
        _deviceRepository = deviceRepository;
        _pingService = pingService;
        _pingResultRepository = pingResultRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var devices = await _deviceRepository.GetEnabledDevicesAsync(cancellationToken);

        foreach (var device in devices)
        {
            var checkResult = await _pingService.CheckAsync(device.IpAddress, cancellationToken);

            var pingResult = new PingResult
            {
                DeviceId = device.Id,
                IpAddress = checkResult.IpAddress,
                Status = checkResult.Status,
                ResponseTimeMs = checkResult.ResponseTimeMs,
                ErrorMessage = checkResult.ErrorMessage,
                CheckedAtUtc = _dateTimeProvider.UtcNow
            };

            await _pingResultRepository.AddAsync(pingResult, cancellationToken);
        }
    }
}
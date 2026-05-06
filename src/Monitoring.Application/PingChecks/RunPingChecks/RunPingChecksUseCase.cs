using Monitoring.Application.Abstractions.Clock;
using Monitoring.Application.Abstractions.Network;
using Monitoring.Application.Abstractions.Persistence;
using Monitoring.Domain.Ping;

namespace Monitoring.Application.PingChecks.RunPingChecks;

public sealed class RunPingChecksUseCase
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IDeviceRepository _deviceRepository;
    private readonly IPingResultRepository _pingResultRepository;
    private readonly IPingService _pingService;

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

    public async Task<RunPingChecksResult> ExecuteAsync(
        CancellationToken cancellationToken)
    {
        var devices = await _deviceRepository.GetEnabledDevicesAsync(cancellationToken);
        var successfulPings = 0;
        var failedPings = 0;
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
            if (pingResult.Status == PingStatus.Success)
            {
                successfulPings++;
            }
            else
            {
                failedPings++;
            }

            await _pingResultRepository.AddAsync(pingResult, cancellationToken);
        }

        return new RunPingChecksResult(
            devices.Count,
            successfulPings,
            failedPings);
    }
}
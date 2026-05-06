using Microsoft.Extensions.Options;
using Monitoring.Application.PingChecks.RunPingChecks;
using Monitoring.PingerWorker.Configurations;

namespace Monitoring.PingerWorker;

public sealed class Worker : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<Worker> _logger;
    private readonly PingerOptions _options;

    public Worker(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<Worker> logger,
        IOptions<PingerOptions> options)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Pinger worker started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();

                var useCase = scope.ServiceProvider
                                   .GetRequiredService<RunPingChecksUseCase>();

                var result = await useCase.ExecuteAsync(stoppingToken);

                _logger.LogInformation(
                    "Ping checks completed. Total: {TotalDevices}, Success: {SuccessfulPings}, Failed: {FailedPings}",
                    result.TotalDevices,
                    result.SuccessfulPings,
                    result.FailedPings);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while running ping checks");
            }

            await Task.Delay(TimeSpan.FromSeconds(_options.IntervalSeconds), stoppingToken);
        }

        _logger.LogInformation("Pinger worker stopped");
    }
}
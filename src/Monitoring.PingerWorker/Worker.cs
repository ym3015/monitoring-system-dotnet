using Monitoring.Application.PingChecks.RunPingChecks;

namespace Monitoring.PingerWorker;

public sealed class Worker : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<Worker> _logger;

    public Worker(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<Worker> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
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

                await useCase.ExecuteAsync(stoppingToken);

                _logger.LogInformation(
                    "Ping checks completed at: {Time}",
                    DateTimeOffset.Now);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while running ping checks");
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }

        _logger.LogInformation("Pinger worker stopped");
    }
}
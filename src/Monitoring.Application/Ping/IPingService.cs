namespace Monitoring.Application.Ping;

public interface IPingService
{
    Task<PingCheckResult> CheckAsync(string ipAddress, CancellationToken cancellationToken);
}
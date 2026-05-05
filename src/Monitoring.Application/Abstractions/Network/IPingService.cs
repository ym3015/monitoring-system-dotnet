using Monitoring.Application.PingChecks.RunPingChecks;

namespace Monitoring.Application.Abstractions.Network;

public interface IPingService
{
    Task<PingCheckResult> CheckAsync(string ipAddress, CancellationToken cancellationToken);
}
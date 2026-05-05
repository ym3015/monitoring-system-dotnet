using Monitoring.Domain.Ping;

namespace Monitoring.Application.Ping;

public interface IPingResultRepository
{
    Task AddAsync(PingResult result, CancellationToken cancellationToken);
}
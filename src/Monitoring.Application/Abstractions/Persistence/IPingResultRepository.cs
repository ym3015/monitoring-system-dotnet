using Monitoring.Domain.Ping;

namespace Monitoring.Application.Abstractions.Persistence;

public interface IPingResultRepository
{
    Task AddAsync(PingResult result, CancellationToken cancellationToken);
}
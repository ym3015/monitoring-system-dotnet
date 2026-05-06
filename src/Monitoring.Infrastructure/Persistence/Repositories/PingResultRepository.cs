using Monitoring.Application.Abstractions.Persistence;
using Monitoring.Domain.Ping;

namespace Monitoring.Infrastructure.Persistence.Repositories;

public sealed class PingResultRepository : IPingResultRepository
{
    private readonly MonitoringDbContext _dbContext;

    public PingResultRepository(MonitoringDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(PingResult result, CancellationToken cancellationToken)
    {
        await _dbContext.PingResults.AddAsync(result, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
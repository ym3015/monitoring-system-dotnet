using Monitoring.Application.Abstractions.Persistence;
using Monitoring.Domain.Ping;

namespace Monitoring.UnitTests.PingChecks.RunPingChecks.Fakes;

internal sealed class FakePingResultRepository : IPingResultRepository
{
    private readonly List<PingResult> _results = new();

    public IReadOnlyList<PingResult> Results => _results;

    public Task AddAsync(PingResult result, CancellationToken cancellationToken)
    {
        _results.Add(result);

        return Task.CompletedTask;
    }
}

using Monitoring.Application.Abstractions.Network;
using Monitoring.Application.PingChecks.RunPingChecks;

namespace Monitoring.UnitTests.PingChecks.RunPingChecks.Fakes;

internal sealed class FakePingService : IPingService
{
    private readonly IReadOnlyDictionary<string, PingCheckResult> _resultsByIpAddress;
    private readonly List<string> _checkedIpAddresses = new();

    public FakePingService(IReadOnlyDictionary<string, PingCheckResult> resultsByIpAddress)
    {
        _resultsByIpAddress = resultsByIpAddress;
    }

    public IReadOnlyList<string> CheckedIpAddresses => _checkedIpAddresses;

    public Task<PingCheckResult> CheckAsync(string ipAddress, CancellationToken cancellationToken)
    {
        _checkedIpAddresses.Add(ipAddress);

        return Task.FromResult(_resultsByIpAddress[ipAddress]);
    }
}

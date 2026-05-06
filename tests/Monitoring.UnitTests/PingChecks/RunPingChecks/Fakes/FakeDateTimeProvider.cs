using Monitoring.Application.Abstractions.Clock;

namespace Monitoring.UnitTests.PingChecks.RunPingChecks.Fakes;

internal sealed class FakeDateTimeProvider : IDateTimeProvider
{
    public FakeDateTimeProvider(DateTime utcNow)
    {
        UtcNow = utcNow;
    }

    public DateTime UtcNow { get; }
}

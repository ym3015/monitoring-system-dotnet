namespace Monitoring.Application.PingChecks.RunPingChecks;

public sealed record RunPingChecksResult(
    int TotalDevices,
    int SuccessfulPings,
    int FailedPings);
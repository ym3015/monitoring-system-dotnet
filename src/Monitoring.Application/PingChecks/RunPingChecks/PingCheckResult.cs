using Monitoring.Domain.Ping;

namespace Monitoring.Application.PingChecks.RunPingChecks;

public sealed record PingCheckResult(
    string IpAddress,
    PingStatus Status,
    long ResponseTimeMs,
    string? ErrorMessage);
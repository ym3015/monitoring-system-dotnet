using Monitoring.Domain.Ping;

namespace Monitoring.Application.Ping;

public sealed record PingCheckResult(
    string IpAddress,
    PingStatus Status,
    long ResponseTimeMs,
    string? ErrorMessage);
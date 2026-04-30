namespace Monitoring.Domain.Ping;

public sealed class PingResult
{
    public long Id { get; set; }

    public int DeviceId { get; set; }

    public string IpAddress { get; set; } = string.Empty;

    public PingStatus Status { get; set; }

    public long ResponseTimeMs { get; set; }

    public string? ErrorMessage { get; set; }

    public DateTime CheckedAtUtc { get; set; }
}
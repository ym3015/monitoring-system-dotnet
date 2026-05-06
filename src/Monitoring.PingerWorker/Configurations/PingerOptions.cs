namespace Monitoring.PingerWorker.Configurations;

public sealed class PingerOptions
{
    public const string SectionName = "Pinger";

    public int IntervalSeconds { get; set; } = 30;
}
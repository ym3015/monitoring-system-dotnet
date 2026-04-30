namespace Monitoring.Domain.Devices;

public sealed class Device
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string IpAddress { get; set; } = string.Empty;

    public bool IsEnabled { get; set; } = false;
}
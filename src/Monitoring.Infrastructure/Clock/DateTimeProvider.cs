using Monitoring.Application.Abstractions.Clock;

namespace Monitoring.Infrastructure.Clock;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
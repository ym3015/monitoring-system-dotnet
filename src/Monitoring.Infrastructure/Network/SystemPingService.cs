using System.Diagnostics;
using System.Net.NetworkInformation;
using Monitoring.Application.Abstractions.Network;
using Monitoring.Application.PingChecks.RunPingChecks;
using Monitoring.Domain.Ping;

namespace Monitoring.Infrastructure.Network;

public sealed class SystemPingService : IPingService
{
    private const int TimeoutMilliseconds = 3000;

    public async Task<PingCheckResult> CheckAsync(
        string ipAddress,
        CancellationToken cancellationToken)
    {
        try
        {
            using var ping = new Ping();

            var stopwatch = Stopwatch.StartNew();

            var reply = await ping.SendPingAsync(ipAddress, TimeoutMilliseconds);

            cancellationToken.ThrowIfCancellationRequested();

            stopwatch.Stop();

            if (reply.Status == IPStatus.Success)
            {
                return new PingCheckResult(
                    ipAddress,
                    PingStatus.Success,
                    stopwatch.ElapsedMilliseconds,
                    null);
            }

            return new PingCheckResult(
                ipAddress,
                PingStatus.Failed,
                stopwatch.ElapsedMilliseconds,
                reply.Status.ToString());
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            return new PingCheckResult(
                ipAddress,
                PingStatus.Failed,
                0,
                ex.Message);
        }
    }
}
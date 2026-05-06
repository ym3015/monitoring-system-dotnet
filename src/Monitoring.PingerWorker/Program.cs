using Monitoring.Application.PingChecks.RunPingChecks;
using Monitoring.Infrastructure;
using Monitoring.PingerWorker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<RunPingChecksUseCase>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();

host.Run();
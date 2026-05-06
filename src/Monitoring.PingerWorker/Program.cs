using Monitoring.Application.PingChecks.RunPingChecks;
using Monitoring.Infrastructure;
using Monitoring.PingerWorker;
using Monitoring.PingerWorker.Configurations;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.Configure<PingerOptions>(builder.Configuration.GetSection(PingerOptions.SectionName));
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<RunPingChecksUseCase>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();

host.Run();
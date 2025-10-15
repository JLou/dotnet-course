using Mythology.Core;
using WorkerService1;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();
builder.Services.AddTransient<IRules, Rules>();
builder.Services.AddTransient<ITournamentProvider, TournamentProvider>();
builder.Services.AddHttpClient();

var host = builder.Build();
host.Run();

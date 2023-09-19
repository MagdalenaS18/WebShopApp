using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

Directory.CreateDirectory("Logs");

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File($"Logs/log-{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.txt", 
        rollingInterval: RollingInterval.Infinite)
    .CreateLogger();

builder.Services.AddControllers();
builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddSerilog();
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod();
        });
});

var app = builder.Build();
app.UseCors();
app.UseSerilogRequestLogging();
app.UseOcelot().Wait();
app.Run();
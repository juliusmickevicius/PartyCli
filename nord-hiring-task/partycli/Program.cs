using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using partycli.Infrastructure.Repository;
using partycli.Services.ArgumentHandlerService;
using partycli.Services.Logger;
using partycli.Services.Server;
using partycli.Settings;
using ILogger = partycli.Services.Logger.ILogger;

namespace partycli
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = Host.CreateApplicationBuilder(args);

            host.Services.AddScoped<ISettingsRepository, SettingsRepository>();
            host.Services.AddScoped<IServersHttpService, ServerHttpService>();
            host.Services.AddScoped<IArgumentHandlerService, ArgumentHandlerService>();
            host.Services.AddScoped<ILogger, Logger>();
            host.Services.AddScoped<App>();

            host.Configuration.AddJsonFile("appsettings.json", false);

            host.Services.Configure<ApiSettings>(options => host.Configuration.GetSection(nameof(ApiSettings)).Bind(options));

            host.Services.AddOptions<ApiSettings>().ValidateOnStart();

            var app = host.Build();

            app.Services.CreateScope().ServiceProvider
                .GetRequiredService<App>()
                .Excecute(args)
                .GetAwaiter()
                .GetResult();
        }
    }
}

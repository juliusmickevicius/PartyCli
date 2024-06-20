using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using partycli.Infrastructure.Repository.Settings;
using partycli.Services.ArgumentHandlerService;
using partycli.Services.MessageDisplay;
using partycli.Services.Server;
using partycli.Settings;

namespace partycli
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = Host.CreateApplicationBuilder(args);

            host.Services.AddSingleton<ISettingsRepository, SettingsRepository>();
            host.Services.AddSingleton<IServersHttpService, ServerHttpService>();
            host.Services.AddSingleton<IArgumentHandlerService, ArgumentHandlerService>();
            host.Services.AddTransient<IMessageDisplayService, MessageDisplayService>();
            host.Services.AddSingleton<App>();

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

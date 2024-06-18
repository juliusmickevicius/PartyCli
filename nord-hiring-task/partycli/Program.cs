using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using partycli.Infrastructure.Repository;
using partycli.Services.Server;

namespace partycli
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((services =>
                {
                    services.AddSingleton<IServerService, ServerService>();
                    services.AddSingleton<App>();
                    services.AddSingleton<ISettingsRepository, SettingsRepository>();
                }));

            var app = host.Build();

            app.Services.CreateScope().ServiceProvider
                .GetRequiredService<App>()
                .Excecute(args)
                .GetAwaiter()
                .GetResult();
        }
    }
}

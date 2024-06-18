using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using partycli.Infrastructure.Repository;
using partycli.Options;
using partycli.Services.Server;

namespace partycli
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = Host.CreateApplicationBuilder(args);

            host.Services.AddSingleton<IServerService, ServerService>();
            host.Services.AddSingleton<App>();
            host.Services.AddSingleton<ISettingsRepository, SettingsRepository>();

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

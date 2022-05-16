
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using AddingAppSettings2.Settings;

namespace AddingAppSettings2;

public class Program
{
    static void Main(string[] args)
    {
        // Setup Host
        var host = CreateDefaultBuilder().Build();

        // Invoke Worker
        using IServiceScope serviceScope = host.Services.CreateScope();
        IServiceProvider provider = serviceScope.ServiceProvider;
        var workerInstance = provider.GetRequiredService<Worker>();
        workerInstance.DoWork();

        host.Run();
    }
    static IHostBuilder CreateDefaultBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(app =>
            {
                app.AddJsonFile("appsettings.json");
            })
            .ConfigureServices(services =>
            {
                services.AddSingleton<Worker>();
            });
    }
    internal class Worker
    {
        private readonly IConfiguration configuration;

        public Worker(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void DoWork()
        {
            var consoleSettings = configuration.GetSection(nameof(ConsoleSettings)).Get<ConsoleSettings>();
        }
    }
}
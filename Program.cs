using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using LungHypertensionApp.Data;

namespace LungHypertensionApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args);
            RunSeeding(host);
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(SetupConfiguration)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void RunSeeding(IHostBuilder host)
        {
            var seederBuilded = host.Build();
            var scopeFactory = seederBuilded.Services.GetService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var lungSeeder = scope.ServiceProvider.GetService<LungHypertensionSeeder>();
                lungSeeder.SeedAsync().Wait(); // Populate initial database
            }
            seederBuilded.Run();
        }

        private static void SetupConfiguration(HostBuilderContext ctx, IConfigurationBuilder builder)
        {
            builder.Sources.Clear();
            builder.AddJsonFile("config.json", false, true)
                   .AddEnvironmentVariables(); // o ovome treba voditi racuna prilikom deploy-a. Prednsot ima poslednji dodat ako ima konflikta medju parametrima
        }
    }
}

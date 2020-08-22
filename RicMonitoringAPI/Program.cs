using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace RicMonitoringAPI
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .WriteTo.File(".\\logs\\RicMonitoringAPI.txt", rollingInterval: RollingInterval.Day)
                    .CreateLogger();

                var host = CreateWebHostBuilder(args).Build();
                host.Run();
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        public static IHostBuilder CreateWebHostBuilder(string[] args) =>

            Host.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var config = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .Build();

                    webBuilder.UseIISIntegration();
                    webBuilder.UseUrls(
                        "https://tenants-api.ericsonramos.com", 
                        $"https://localhost:{config.GetValue<int>("Host:Port")}",
                        $"https://tenants-api:{config.GetValue<int>("Host:Port")}");
                    webBuilder.ConfigureKestrel(serverOptions => { serverOptions.AddServerHeader = false; });
                    webBuilder.UseStartup<Startup>();
                });
    }
}

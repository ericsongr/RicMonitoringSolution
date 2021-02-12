using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RicAuthJwtServer.Data;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace RicAuthJwtServer
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
                    .WriteTo.File(".\\logs\\RicAuthJwtServer.txt", rollingInterval: RollingInterval.Day)
                    .CreateLogger();

                var host = CreateWebHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        SeedData.UserData(scope);
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred while migrating and seeding the database.");
                    }
                }

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

                    webBuilder.UseUrls("https://authserver.ericsonramos.com/");

                    webBuilder.ConfigureKestrel(serverOptions => { serverOptions.AddServerHeader = false; });

                    webBuilder.UseStartup<Startup>();

                    webBuilder.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                        .ReadFrom.Configuration(hostingContext.Configuration)
                        .MinimumLevel.Debug()
                        .Enrich.FromLogContext()
                        .WriteTo.File(".\\logs\\RicAuthJwtServer.txt", rollingInterval: RollingInterval.Day)
                        .WriteTo.Console(theme: AnsiConsoleTheme.Code));
                });

    }
}

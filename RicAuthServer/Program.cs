using System;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using RicAuthServer.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog.Events;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace RicAuthServer
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            //var seed = args.Contains("/seed");
            //if (seed)
            //{
            //    args = args.Except(new[] {"/seed"}).ToArray();
            //}

            //seed = false;

            try
            {
                Log.Information("Starting web host");

                //var host = CreateWebHostBuilder(args);
                var host = CreateHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    try
                    {
                       

                        SeedData.EnsureSeedData(services);

                        //seeding the user - identity user
                        //var userManager = scope.ServiceProvider
                        //    .GetService<UserManager<ApplicationUser>>();
                        //var user = new ApplicationUser();
                        //user.UserName = "ericson";
                        //var result = userManager.CreateAsync(user, "ramos").GetAwaiter().GetResult();
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
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                    {
                        config
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: false, reloadOnChange: true);
                        context.Configuration = config.Build();
                    })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseIISIntegration();
                    webBuilder.ConfigureKestrel(serverOptions => { serverOptions.AddServerHeader = false; });
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                        .ReadFrom.Configuration(hostingContext.Configuration)
                        .MinimumLevel.Debug()
                        .Enrich.FromLogContext()
                        .WriteTo.Console(theme: AnsiConsoleTheme.Code));
                });

        //WebHost
        //    .CreateDefaultBuilder(args)
        //    .ConfigureAppConfiguration((context, config) =>
        //    {
        //        config
        //            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //            .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: false, reloadOnChange: true);
        //        context.Configuration = config.Build();
        //    })
        //    .UseStartup<Startup>()
        //    .UseKestrel(options =>
        //    {
        //        options.AddServerHeader = false;
        //        //options.Listen(IPAddress.Any, 443, listenOptions =>
        //        //{
        //        //    listenOptions.UseHttps("server.pfx", "password");
        //        //});
        //    })
        //    .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
        //        .ReadFrom.Configuration(hostingContext.Configuration)
        //        .MinimumLevel.Debug()
        //        .Enrich.FromLogContext()
        //        .WriteTo.Console(theme: AnsiConsoleTheme.Code)
        //    //.WriteTo.RollingFile("Log")
        //    )
        //    .UseIISIntegration()
        //    .Build();
    }
}

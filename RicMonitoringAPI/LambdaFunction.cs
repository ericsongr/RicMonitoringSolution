using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace RicMonitoringAPI
{
    public class LambdaFunction : Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction
    {
        protected override void Init(IWebHostBuilder builder)
        {
            //IWebHostBuilder webHostBuilder = builder
            //    .UseContentRoot(Directory.GetCurrentDirectory())
            //    .UseStartup<Startup>()
            //    .UseApiGateway();
            builder
                .UseStartup<Startup>();
        }
    }
}

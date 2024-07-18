using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Serilog;

namespace RicMonitoringAPI
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAllServicesFromAssemblies(this IServiceCollection services,  params Assembly[] assemblies)
        {
            // Or use scoped/singleton as needed
            //ServiceLifetime.Transient
            //ServiceLifetime.Scoped
            //ServiceLifetime.Singleton

            foreach (var assembly in assemblies)
            {
                // Find all types that are interfaces and their implementations
                var serviceTypes = assembly.GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract)
                    .SelectMany(t => t.GetInterfaces(), (impl, service) => new { Service = service, Implementation = impl })
                    .Where(x => x.Service.Name == "I" + x.Implementation.Name)
                    .ToList();

                // Register each service with the specified lifetime
                foreach (var serviceType in serviceTypes)
                {
                    
                    if (serviceType.Service.FullName != null && serviceType.Service.FullName.EndsWith("Repository"))
                    {
                        services.Add(new ServiceDescriptor(serviceType.Service, serviceType.Implementation, ServiceLifetime.Scoped));
                    }
                    else if (serviceType.Service.FullName != null && serviceType.Service.FullName.EndsWith("Service"))
                    {
                        services.Add(new ServiceDescriptor(serviceType.Service, serviceType.Implementation, ServiceLifetime.Transient));
                    }
                }

            }

            return services;
        }
    }
}

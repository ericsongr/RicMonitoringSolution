using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;
using RicAuthServer.Data.Migrations.IdentityServer;

namespace RicAuthServer.Data
{
    public class SeedData
    {
        public static void EnsureSeedData(IServiceProvider serviceProvider)
        {
            Console.WriteLine("Seeding database...");
            PerformMigrations(serviceProvider);

            EnsureSeedData(serviceProvider.GetRequiredService<ConfigurationDbContext>());
            Console.WriteLine("Done seeding database.");
        }

        private static void PerformMigrations(IServiceProvider serviceProvider)
        {
            serviceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
            serviceProvider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();
            serviceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
        }

        private static void EnsureSeedData(ConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                Console.WriteLine("Clients being populated");
                var clients = Config.GetClients().ToList();
                foreach (var client in clients)
                {
                    var clientEntity = client.ToEntity();
                    context.Clients.Add(clientEntity);
                }

                try
                {
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
              
            }
            else
            {
                Console.WriteLine("Clients already populated");
            }

            if (!context.IdentityResources.Any())
            {
                Console.WriteLine("IdentityResources being populated");
                foreach (var resource in Config.GetIdentityResources().ToList())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("IdentityResources already populated");
            }

            if (!context.ApiResources.Any())
            {
                Console.WriteLine("ApiResources being populated");
                foreach (var resource in Config.GetApiResources().ToList())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("ApiResources already populated");
            }
        }

        public static void UserData(IServiceScope scope)
        {
            //seeding the user - identity user
            var userManager = scope.ServiceProvider
                .GetService<UserManager<ApplicationUser>>();

            var user = new ApplicationUser();
            user.UserName = "ericson";
            var result = userManager.CreateAsync(user, "ramos").GetAwaiter().GetResult();

            user = new ApplicationUser();
            user.UserName = "sherine";
            result = userManager.CreateAsync(user, "ramos").GetAwaiter().GetResult();

            user = new ApplicationUser();
            user.UserName = "egboy";
            user.FirstName = "Eldric Gesua";
            user.LastName = "Ramos";
            result = userManager.CreateAsync(user, "ramos").GetAwaiter().GetResult();


            var roleManager = scope.ServiceProvider
                .GetService<RoleManager<IdentityRole>>();

            string superuser = "Superuser";
            string administrator = "Administrator";
            if (!roleManager.RoleExistsAsync(superuser).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole(superuser)).GetAwaiter().GetResult();
            }

            if (!roleManager.RoleExistsAsync(administrator).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole(administrator)).GetAwaiter().GetResult();
            }

            //seeding the user role
            var ericsonRole = userManager.FindByNameAsync("ericson").GetAwaiter().GetResult();
            if (!userManager.IsInRoleAsync(ericsonRole, superuser).GetAwaiter().GetResult())
            {
                userManager.AddToRoleAsync(ericsonRole, superuser).GetAwaiter().GetResult();
            }

            var sherineRole = userManager.FindByNameAsync("sherine").GetAwaiter().GetResult();
            if (!userManager.IsInRoleAsync(sherineRole, administrator).GetAwaiter().GetResult())
            {
                userManager.AddToRoleAsync(sherineRole, administrator).GetAwaiter().GetResult();
            }

            var egboyRole = userManager.FindByNameAsync("egboy").GetAwaiter().GetResult();
            if (!userManager.IsInRoleAsync(egboyRole, administrator).GetAwaiter().GetResult())
            {
                userManager.AddToRoleAsync(egboyRole, administrator).GetAwaiter().GetResult();
            }

            //seeding claim
            var resultClaim = userManager.AddClaimAsync(user, new Claim("ric.test", "big.cookie"))
                .GetAwaiter().GetResult();

            var resultClaim2 = userManager.AddClaimAsync(user, new Claim("ric.api.test", "big.api.cookie"))
                .GetAwaiter().GetResult();
        }

    }
}

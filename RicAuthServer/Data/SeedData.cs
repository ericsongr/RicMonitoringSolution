using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;
using RicAuthServer.Data.Constants;
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
            user.FirstName = "Ericson";
            user.LastName = "Ramos";
            var result = userManager.CreateAsync(user, "ramos").GetAwaiter().GetResult();

            user = new ApplicationUser();
            user.UserName = "sherine";
            user.FirstName = "Sherine";
            user.LastName = "Ramos";
            result = userManager.CreateAsync(user, "ramos").GetAwaiter().GetResult();

            user = new ApplicationUser();
            user.UserName = "egboy";
            user.FirstName = "Eldric Gesua";
            user.LastName = "Ramos";
            result = userManager.CreateAsync(user, "ramos").GetAwaiter().GetResult();


            var roleManager = scope.ServiceProvider
                .GetService<RoleManager<IdentityRole>>();

            if (!roleManager.RoleExistsAsync(RoleConstant.Superuser).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole(RoleConstant.Superuser)).GetAwaiter().GetResult();
            }

            if (!roleManager.RoleExistsAsync(RoleConstant.Administrator).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole(RoleConstant.Administrator)).GetAwaiter().GetResult();
            }

            if (!roleManager.RoleExistsAsync(RoleConstant.Staff).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole(RoleConstant.Staff)).GetAwaiter().GetResult();
            }

            //seeding the user role
            var ericsonRole = userManager.FindByNameAsync("ericson").GetAwaiter().GetResult();
            if (ericsonRole != null && !userManager.IsInRoleAsync(ericsonRole, RoleConstant.Superuser).GetAwaiter().GetResult())
            {
                userManager.AddToRoleAsync(ericsonRole, RoleConstant.Superuser).GetAwaiter().GetResult();
            }

            var sherineRole = userManager.FindByNameAsync("sherine").GetAwaiter().GetResult();
            if (sherineRole != null && !userManager.IsInRoleAsync(sherineRole, RoleConstant.Administrator).GetAwaiter().GetResult())
            {
                userManager.AddToRoleAsync(sherineRole, RoleConstant.Administrator).GetAwaiter().GetResult();
            }

            var egboyRole = userManager.FindByNameAsync("egboy").GetAwaiter().GetResult();
            if (egboyRole != null && !userManager.IsInRoleAsync(egboyRole, RoleConstant.Administrator).GetAwaiter().GetResult())
            {
                userManager.AddToRoleAsync(egboyRole, RoleConstant.Administrator).GetAwaiter().GetResult();
            }

            ////seeding claim
            //var resultClaim = userManager.AddClaimAsync(user, new Claim("ric.test", "big.cookie"))
            //    .GetAwaiter().GetResult();

            //var resultClaim2 = userManager.AddClaimAsync(user, new Claim("ric.api.test", "big.api.cookie"))
            //    .GetAwaiter().GetResult();
        }

    }
}

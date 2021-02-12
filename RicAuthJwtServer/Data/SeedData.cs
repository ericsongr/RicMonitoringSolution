using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;

namespace RicAuthJwtServer.Data
{
    public class SeedData
    {

        public static void UserData(IServiceScope scope)
        {
            //seeding the user - identity user
            var userManager = scope.ServiceProvider
                .GetService<UserManager<ApplicationUser>>();

            var user = new ApplicationUser();
            user.UserName = "ericson";
            user.FirstName = "Ericson";
            user.LastName = "Ramos";
            user.Email = "ericson@yahoo.com";
            user.NormalizedEmail = "ericson@yahoo.com";
            var result = userManager.CreateAsync(user, "ramos").GetAwaiter().GetResult();

            user = new ApplicationUser();
            user.UserName = "sherine";
            user.FirstName = "Sherine";
            user.LastName = "Ramos";
            user.Email = "sherine@yahoo.com";
            user.NormalizedEmail = "sherine@yahoo.com";

            result = userManager.CreateAsync(user, "ramos").GetAwaiter().GetResult();

            user = new ApplicationUser();
            user.UserName = "egboy";
            user.FirstName = "Eldric Gesua";
            user.LastName = "Ramos";
            user.Email = "egboy@yahoo.com";
            user.NormalizedEmail = "egboy@yahoo.com";

            result = userManager.CreateAsync(user, "ramos").GetAwaiter().GetResult();

            user = new ApplicationUser();
            user.UserName = "RunDailyBatch";
            user.FirstName = "RunDailyBatch";
            user.LastName = "RunDailyBatch";
            user.Email = "RunDailyBatch@yahoo.com";
            user.NormalizedEmail = "RunDailyBatch@yahoo.com";

            result = userManager.CreateAsync(user, "Pa$$w0rd").GetAwaiter().GetResult();

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

            if (!roleManager.RoleExistsAsync(RoleConstant.RunDailyBatch).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole(RoleConstant.RunDailyBatch)).GetAwaiter().GetResult();
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

            var runDailyBatch = userManager.FindByNameAsync("RunDailyBatch").GetAwaiter().GetResult();
            if (runDailyBatch != null && !userManager.IsInRoleAsync(runDailyBatch, RoleConstant.RunDailyBatch).GetAwaiter().GetResult())
            {
                userManager.AddToRoleAsync(runDailyBatch, RoleConstant.RunDailyBatch).GetAwaiter().GetResult();
            }

        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using RicAuthServer.Data;

namespace RicAuthServer.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;

        public ProfileService(
            UserManager<ApplicationUser> userManager,
            IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _claimsFactory = claimsFactory ?? throw new ArgumentNullException(nameof(claimsFactory));
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = _userManager.FindByIdAsync(sub).GetAwaiter().GetResult();
            var principal = _claimsFactory.CreateAsync(user).GetAwaiter().GetResult();

            var claims = principal.Claims.ToList();
            claims = claims.Where(o => context.RequestedClaimTypes.Contains(o.Type)).ToList();

            //add first name and last name
            claims.Add(new Claim("FullName", $"{user.FirstName} {user.LastName}"));

            //user name
            claims.Add(new Claim("UserName", user.UserName));

            //add requested claims and first, last name
            context.IssuedClaims.AddRange(claims);

            //List<string> list = context.RequestedClaimTypes.ToList();
            //add roles here
            var roleClaims = context.Subject.FindAll(JwtClaimTypes.Role);
            context.IssuedClaims.AddRange(roleClaims);

            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.CompletedTask;
        }
    }
}

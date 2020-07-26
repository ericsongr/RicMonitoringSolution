using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace RicAuthServer.RicAuthControllers.ExtensionMethods
{
    public static class IdentityUserErrors
    {
        public static object ShowErrors(this IdentityResult results)
        {
            return results.Errors.Select(o => new
            {
                Code = o.Code,
                Description = o.Description
            });
        }
    }
}

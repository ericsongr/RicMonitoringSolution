using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace RicAuthServer
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("RicMonitoringAPI", "RicMonitoringAPI Application")
            };
        }

        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("roles", new [] {"role"})
            };
        }

        //private static string spaClientUrl = "https://localhost:44311";
        private static string spaClientUrl = "http://localhost:4200";

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                //new Client
                //{
                //    ClientId = "clientApp",
 
                //    // no interactive user, use the clientid/secret for authentication
                //    AllowedGrantTypes = GrantTypes.ClientCredentials,
 
                //    // secret for authentication
                //    ClientSecrets =
                //    {
                //        new Secret("secret".Sha256())
                //    },
 
                //    // scopes that client has access to
                //    AllowedScopes = { "resourceApi" }
                //},

                //// OpenID Connect implicit flow client (MVC)
                //new Client
                //{
                //    ClientId = "mvc",
                //    ClientName = "MVC Client",
                //    AllowedGrantTypes = GrantTypes.Hybrid,

                //    ClientSecrets =
                //    {
                //        new Secret("secret".Sha256())
                //    },

                //    RedirectUris = { $"https://localhost:4200/signin-oidc" },
                //    PostLogoutRedirectUris = { $"https://localhost:4200/signout-callback-oidc" },
                //    AllowedScopes =
                //    {
                //        IdentityServerConstants.StandardScopes.OpenId,
                //        IdentityServerConstants.StandardScopes.Profile,
                //        "resourceApi"
                //    },
                //    AllowOfflineAccess = true
                //},
                //new Client
                //{
                //    ClientId = "spaClient",
                //    ClientName = "SPA Client",
                //    AllowedGrantTypes = GrantTypes.Implicit,
                //    AllowAccessTokensViaBrowser = true,

                //    RedirectUris = { $"{spaClientUrl}/callback" },
                //    PostLogoutRedirectUris = { $"{spaClientUrl}/" },
                //    AllowedCorsOrigins = { $"{spaClientUrl}" },
                //    AllowedScopes =
                //    {
                //        IdentityServerConstants.StandardScopes.OpenId,
                //        IdentityServerConstants.StandardScopes.Profile, 
                //        "resourceApi"
                //    }
                //},
                 new Client
                {
                    ClientId = "spaRicMonitoringCodeClient",
                    ClientName = "SPA Ric Monitoring Code Client",
                    AccessTokenType = AccessTokenType.Jwt,
                    // RequireConsent = false,
                    AccessTokenLifetime = 330,// 330 seconds, default 60 minutes
                    IdentityTokenLifetime = 30,

                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,

                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = new List<string>
                    {
                        $"{spaClientUrl}/callback",
                        $"{spaClientUrl}",
                        $"{spaClientUrl}/silent-renew.html"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        $"{spaClientUrl}/unauthorized",
                        $"{spaClientUrl}"
                    },
                    AllowedCorsOrigins = new List<string>
                    {
                        $"{spaClientUrl}"
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "RicMonitoringAPI"
                    },
                    RequireConsent = false,
                },
                
            };
        }

    }
}

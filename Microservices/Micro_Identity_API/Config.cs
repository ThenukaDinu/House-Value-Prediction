using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Micro_Authentication_API
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
               {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
                    new IdentityResource
                    {
                        Name = "role",
                        UserClaims = new List<string> {"role"}
                    }
               };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("all"),
                new ApiScope("read"),
                new ApiScope("write"),
                new ApiScope("update"),
                new ApiScope("delete"),
            };
        public static IEnumerable<ApiResource> ApiResources => new[]
        {
            new ApiResource("weatherapi")
        {
            Scopes = new List<string> { "all", "read", "write", "update", "delete" },
            ApiSecrets = new List<Secret> {new Secret("ScopeSecret".Sha256())},
            UserClaims = new List<string> {"role"}
        }
        };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "m2m.client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    AllowedScopes = { "all", "read", "write", "update", "delete" }
                },

                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "interactive",
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "https://localhost:44311/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:44311/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:44311/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "all", "read", "write", "update", "delete" }
                },
                new Client
                {
                    ClientId = "VueWebApp",
                    ClientName = "VueWebApp JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = {
                        "http://localhost:44344/auth/callback",
                        "http://localhost:44344/static/silent-renew.html"
                    },
                    PostLogoutRedirectUris = { "http://localhost:44344/" },
                    AllowedCorsOrigins = { "http://localhost:44344", "https://localhost:44344" },
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 90, // 1.5 minutes
                    AbsoluteRefreshTokenLifetime = 0,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    RequireConsent = false,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "all", "read", "write", "update", "delete"
                    }
                }
            };
    }
}

using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;


namespace WT.IDP.Infrastructure.IDPSetup
{
    //https://nahidfa.com/posts/migrating-identityserver4-to-v4/
    public static class IdentityServerConfiguration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
           new List<IdentityResource>
           {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name="wt.scope",
                    UserClaims =
                    {
                        "wt.Tenant",
                        "Wt.UserType"
                    }
                }
           };

        public static IEnumerable<ApiScope> GetApiScopes() =>
            new List<ApiScope>
            {
                // backward compat
                new ApiScope("WT.Ecommerce.WebAPI")
            };

        public static IEnumerable<ApiResource> GetApis() =>
           new List<ApiResource> {
                new ApiResource("WT.Ecommerce.WebAPI",new string[]{"Wt.UserType","Wt.UserType"})
                {
                   Scopes = new []{ "WT.Ecommerce.WebAPI" }
                }
           };

        public static IEnumerable<Client> GetClient() =>
           new List<Client>
           {
                new Client
                {
                    ClientId="WTEcommerce_id_mvc",
                    ClientSecrets={new Secret("clinet_secret_mvc_123".ToSha256())},
                    AllowedGrantTypes=GrantTypes.Code,
                    RequirePkce=true,
                    RedirectUris={"https://localhost:44387/signin-oidc" },
                    PostLogoutRedirectUris={"https://localhost:44387/home/index" },
                    AllowedScopes={"WT.Ecommerce.WebAPI",
                                   IdentityServerConstants.StandardScopes.OpenId,
                                   IdentityServerConstants.StandardScopes.Profile,
                                   "wt.scope"
                                  },
                    // puts all the claims in the id token
                    //AlwaysIncludeUserClaimsInIdToken=true,
                    AllowOfflineAccess=true,
                    RequireConsent=false
                },
                new Client
                {
                    ClientId="WTEcommerceClientJs_id_js",
                    ClientSecrets={new Secret("clinet_secret_js_123".ToSha256())},
                    AllowedGrantTypes=GrantTypes.Code,
                    RequirePkce=true,
                    RequireClientSecret=false,
                    RedirectUris={"https://localhost:44366/home/signIn" },
                    PostLogoutRedirectUris={"https://localhost:44366/home/index" },
                    AllowedCorsOrigins={"https://localhost:44366"},
                    AllowedScopes={"WT.Ecommerce.WebAPI",
                                   IdentityServerConstants.StandardScopes.OpenId,
                                   IdentityServerConstants.StandardScopes.Profile,
                                   "wt.scope"
                                  },
                    AccessTokenLifetime=1,
                    // puts all the claims in the id token
                    //AlwaysIncludeUserClaimsInIdToken=true,
                    AllowAccessTokensViaBrowser=true,
                    RequireConsent=false
                }
           };

    }
}

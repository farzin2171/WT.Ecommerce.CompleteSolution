using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WT.Ecommerce.Infrastructure.Installers
{
    public class AuthInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {

            services.AddAuthentication(config =>
            {
                config.DefaultScheme = "Cookie";
                config.DefaultChallengeScheme = "oidc";

            })
              .AddCookie("Cookie")
              .AddOpenIdConnect("oidc", config =>
              {
                  config.Authority = "https://localhost:6001/";
                  config.ClientId = "WTEcommerce_id_mvc";
                  config.ClientSecret = "clinet_secret_mvc_123";
                  config.SaveTokens = true;
                  config.UsePkce = true;
                  config.ResponseType = "code";

                  config.SignedOutCallbackPath = "/Home/Index";
                  //configure cookie claim mapping
                  config.ClaimActions.DeleteClaim("amr");
                  config.ClaimActions.DeleteClaim("s_hash");
                  config.ClaimActions.MapUniqueJsonKey("wt.AppUserType", "wt.UserType");
                  //two trips to load claims in the cookie
                  //but the id is smaller
                  config.GetClaimsFromUserInfoEndpoint = true;

                  //configure scope
                  config.Scope.Clear();
                  config.Scope.Add("openid");
                  config.Scope.Add("wt.scope");
                  config.Scope.Add("WT.Ecommerce.WebAPI");
                  config.Scope.Add("offline_access");


              });
        }
    }
}

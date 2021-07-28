using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WT.Ecommerce
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(config =>
            {
                config.DefaultScheme = "Cookie";
                config.DefaultChallengeScheme = "oidc";

            })
              .AddCookie("Cookie")
              .AddOpenIdConnect("oidc", config =>
              {
                  config.Authority = "https://localhost:44361/";
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
            services.AddHttpClient();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

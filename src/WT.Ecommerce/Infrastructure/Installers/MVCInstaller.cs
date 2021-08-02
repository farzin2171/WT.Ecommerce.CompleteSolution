using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WT.Ecommerce.Infrastructure.Filters;

namespace WT.Ecommerce.Infrastructure.Installers
{
    public class MVCInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<ValidationFilter>();
            }).AddFluentValidation(mvcConfiguration => mvcConfiguration.RegisterValidatorsFromAssemblyContaining<Startup>()); ;
        }
    }
}

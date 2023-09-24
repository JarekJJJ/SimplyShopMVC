using Microsoft.Extensions.DependencyInjection;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application
{
    public static class DependencyInjection
    {
       public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IArticleService, ArticleService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}

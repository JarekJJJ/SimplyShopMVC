using Microsoft.Extensions.DependencyInjection;
using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IArticleRepository, ArticleRepository>();
            services.AddTransient<IItemRepository, ItemRepository>();
        return services;
        }
         
    }
}

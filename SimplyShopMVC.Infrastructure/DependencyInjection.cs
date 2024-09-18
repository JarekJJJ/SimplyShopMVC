using Microsoft.Extensions.Configuration;
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
            services.AddTransient<ISupplierRepository, SupplierRepository>();
            services.AddTransient<IGroupItemRepository, GroupItemRepository>();
            services.AddTransient<IOmnibusPriceRepository, OmnibusPriceRepository>();
            services.AddTransient<ICategoryTagsRepository, CategoryTagsRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ICompanySettingsRepository, CompanySettingsRepository>();
            services.AddTransient<IOrinkRepository, OrinkRepository>();
            services.AddTransient<ISetsRepository, SetsRepository>();
            services.AddTransient<IDeliveryRepository, DeliveryRepository>();
            services.AddTransient<IFavoriteItemRepository, FavoriteItemRepository>();
            services.AddTransient<IMessageRepository, MessageRepository>();
            return services;
        }


    }
}

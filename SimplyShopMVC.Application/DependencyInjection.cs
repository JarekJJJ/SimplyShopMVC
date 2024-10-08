﻿using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SimplyShopMVC.Application.ViewModels.Article;
using SimplyShopMVC.Application.ViewModels.Item;
using static SimplyShopMVC.Application.ViewModels.Item.AddItemVm;
using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Application.Helpers;

namespace SimplyShopMVC.Application
{
    public static class DependencyInjection
    {
       public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // FluentValidation
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<NewArticleValidation>();
            services.AddValidatorsFromAssemblyContaining<AddItemValidation>();

            //services
            services.AddTransient<IArticleService, ArticleService>();
            services.AddTransient<IItemService, ItemService>();
            services.AddTransient<ISupplierService, SupplierService>();
            services.AddTransient<IFrontService, FrontService>();
            services.AddTransient<IOmnibusHelper, OmnibusHelper>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IGeneratePdf, GeneratePdf>();
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<IPriceCalculate, PriceCalculate>();
            services.AddTransient<ISetService, SetService>();
            services.AddTransient<IMessageService, MessageService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddHttpClient();
            return services;
        }
    }
}

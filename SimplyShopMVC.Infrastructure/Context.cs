using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimplyShopMVC.Domain.Model;
using SimplyShopMVC.Domain.Model.Messages;
using SimplyShopMVC.Domain.Model.Order;
using SimplyShopMVC.Domain.Model.Sets;
using SimplyShopMVC.Domain.Model.Suppliers;
using SimplyShopMVC.Domain.Model.users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Infrastructure
{
    public class Context : IdentityDbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ConnectArticleTag> ConnectArticleTag { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemWarehouse> ItemWarehouses { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<ItemTag> ItemTags { get; set; }
        public DbSet<ConnectItemTag> ConnectItemTag { get; set; }
        public DbSet<VatRate> VatRates { get; set; }
        public DbSet<Incom> Incoms { get; set; }
        public DbSet<IncomGroup> IncomGroups { get; set; }
        public DbSet<GroupItem> GroupItems { get; set; }
        public DbSet<OmnibusPrice> OmnibusPrices { get; set; }
        public DbSet<ConnectCategoryTag> ConnectCategoryTags { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItems> CartsItems { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<CompanySettings> CompanySettings { get; set; }
        public DbSet<Orink> Orinks { get; set; }
        public DbSet<OrinkGroup> OrinkGroups { get; set; }
        public DbSet<PcSets> PcSets { get; set; }
        public DbSet<PcSetsItems> PcSetsItems { get; set; }
        public DbSet<Delivery> Delivery { get; set; }
        public DbSet<ConnectCategoryGroup> ConnectCategoryGroups { get; set; }
        public DbSet<FavoriteItem> FavoriteItems { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageTicket> MessageTickets { get; set; }
        public Context(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ConnectCategoryTag>()
                .HasKey(cct => new { cct.CategoryId, cct.ItemTagId});
            builder.Entity<ConnectCategoryTag>()
                .HasOne<Category>(cct => cct.Category)
                .WithMany(b => b.ConnectCategoryTag)
                .HasForeignKey(cct => cct.CategoryId);
            builder.Entity<ConnectCategoryTag>()
                .HasOne<ItemTag>(cct => cct.ItemTag)
                .WithMany(c => c.ConnectCategoryTag)
                .HasForeignKey(cct => cct.ItemTagId);

            builder.Entity<ConnectArticleTag>()
                .HasKey(cat => new { cat.ArticleId, cat.ArticleTagId });
            
            builder.Entity<ConnectArticleTag>()
                .HasOne<Article>(cat=>cat.Article)
                .WithMany(a=>a.ConnectArticleTags)
                .HasForeignKey(cat=>cat.ArticleId);

            builder.Entity<ConnectArticleTag>()
                .HasOne<ArticleTag>(cat => cat.ArticleTag)
                .WithMany(at => at.ConnectArticleTags)
                .HasForeignKey(cat => cat.ArticleTagId);

            builder.Entity<ConnectItemTag>()
               .HasKey(cit => new { cit.ItemId, cit.ItemTagId });

            builder.Entity<ConnectItemTag>()
                .HasOne<Item>(cit => cit.Item)
                .WithMany(i => i.ConnectItemTags)
                .HasForeignKey(cit => cit.ItemId);

            builder.Entity<ConnectItemTag>()
                .HasOne<ItemTag>(cit => cit.ItemTag)
                .WithMany(it => it.ConnectItemTags)
                .HasForeignKey(cit => cit.ItemTagId);
        }
    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimplyShopMVC.Domain.Model;
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
        public DbSet<ItemTag> ItemTags { get; set; }
        public DbSet<ConnectItemTag> ConnectItemTag { get; set; }

        public Context(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
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

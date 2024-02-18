using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using SimplyShopMVC.Application;
using SimplyShopMVC.Application.ViewModels.Article;
using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Infrastructure;
using SimplyShopMVC.Infrastructure.Repositories;
using SimplyShopMVC.Web.Services;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var emailConfiguration = new EmailConfiguration();
builder.Configuration.GetSection("EmailSettings").Bind(emailConfiguration);
builder.Services.AddSingleton<IEmailConfiguration>(emailConfiguration);
builder.Services.AddScoped<IEmailSender, EmailSenderService>();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<Context>();
//Tutaj trzeba powiązać interfejsy z repository !!!
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.AddControllersWithViews();
//Fluent Validation przeniesiono do DependencyInjection w Aplication
builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;

});
//lekcja 9.8, 9.10
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanEditArticle", policy =>
    {
        policy.RequireClaim("EditArticle");
        policy.RequireRole("Admin");
    });
});
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "blog",
    pattern: "{controller=Article}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "Admin",
    pattern: "{controller=Shop}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "Sklep",
    pattern: "{controller=Item}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "Supplier",
    pattern: "{controller=Supplier}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

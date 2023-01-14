using Domain.EF;
using Domain.Entities;
using FurnitureWeb.APICaller.Brand;
using FurnitureWeb.APICaller.CartItem;
using FurnitureWeb.APICaller.Category;
using FurnitureWeb.APICaller.Discount;
using FurnitureWeb.APICaller.Order;
using FurnitureWeb.APICaller.Product;
using FurnitureWeb.APICaller.ReviewItem;
using FurnitureWeb.APICaller.User;
using FurnitureWeb.APICaller.WishItem;
using FurnitureWeb.Services.External.Paypal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
// Add services to the container.
services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("AppDbContext")));

services.AddIdentityCore<AppUser>()
    .AddRoles<IdentityRole>()
    .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<AppUser, IdentityRole>>()
    .AddEntityFrameworkStores<AppDbContext>();

services.AddScoped<ICategoryAPIClient, CategoryAPIClient>();
services.AddScoped<IBrandAPIClient, BrandAPIClient>();
services.AddScoped<IProductAPIClient, ProductAPIClient>();
services.AddScoped<IUserAPIClient, UserAPIClient>();
services.AddScoped<IOrderAPIClient, OrderAPIClient>();
services.AddScoped<ICartItemAPIClient, CartItemAPIClient>();
services.AddScoped<IWishItemAPIClient, WishItemAPIClient>();
services.AddScoped<IPaypalService, PaypalService>();
services.AddScoped<IReviewItemAPIClient, ReviewItemAPIClient>();
services.AddScoped<IDiscountAPIClient, DiscountAPIClient>();
services.TryAddScoped<SignInManager<AppUser>>();
services.AddHttpClient();
services.AddSingleton<IConfiguration>(sp =>
{
    IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
    configurationBuilder.AddJsonFile("appsettings.json");
    return configurationBuilder.Build();
});
string issuer = configuration.GetValue<string>("Tokens:Issuer");
string signingKey = configuration.GetValue<string>("Tokens:Key");
byte[] signingKeyBytes = Encoding.UTF8.GetBytes(signingKey);
services
    .AddAuthentication("UserAuth")
    .AddJwtBearer(opts =>
    {
        opts.RequireHttpsMetadata = false;
        opts.SaveToken = true;
        opts.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = issuer,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
        };
    })
    .AddCookie("UserAuth", options =>
    {
        options.LoginPath = "/signin";
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
        options.AccessDeniedPath = "/signin";
        options.LogoutPath = "/signout";
        options.Cookie.Name = "User";
    })
    .AddCookie(IdentityConstants.ExternalScheme, options =>
    {
        options.LoginPath = "/signin";
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
        options.AccessDeniedPath = "/signin";
        options.LogoutPath = "/signout";
        options.Cookie.Name = "GoogleUser";
    })
    .AddGoogle(opts =>
    {
        IConfigurationSection googleAuthNSection = configuration.GetSection("Authentication:Google");
        opts.ClientId = googleAuthNSection["ClientId"];
        opts.ClientSecret = googleAuthNSection["ClientSecret"];
        opts.SignInScheme = IdentityConstants.ExternalScheme;
    });
services.AddAuthorization();
services.AddDistributedMemoryCache();

services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => false; //true;
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
});
services.AddHttpContextAccessor();
services.AddControllersWithViews();

var app = builder.Build();
var env = app.Environment;
// Configure the HTTP request pipeline.
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
//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();

app.UseAuthorization();
app.UseCookiePolicy();
app.UseSession();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
using Domain.EF;
using Domain.Entities;
using FurnitureWeb.APICaller.Brand;
using FurnitureWeb.APICaller.Category;
using FurnitureWeb.APICaller.Discount;
using FurnitureWeb.APICaller.Order;
using FurnitureWeb.APICaller.Product;
using FurnitureWeb.APICaller.ReviewItem;
using FurnitureWeb.APICaller.Role;
using FurnitureWeb.APICaller.User;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
// Add services to the container.
services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("AppDbContext")));

services.AddIdentityCore<AppUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

services.AddScoped<IBrandAPIClient, BrandAPIClient>();
services.AddScoped<ICategoryAPIClient, CategoryAPIClient>();
services.AddScoped<IProductAPIClient, ProductAPIClient>();
services.AddScoped<IUserAPIClient, UserAPIClient>();
services.AddScoped<IRoleAPIClient, RoleAPIClient>();
services.AddScoped<IDiscountAPIClient, DiscountAPIClient>();
services.AddScoped<IOrderAPIClient, OrderAPIClient>();
services.AddScoped<IReviewItemAPIClient, ReviewItemAPIClient>();
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
    .AddAuthentication("AdminAuth")
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
    .AddCookie("AdminAuth", options =>
    {
        options.LoginPath = "/admin/login";
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
        options.AccessDeniedPath = "/admin/login";
        options.LogoutPath = "/admin/logout";
        options.Cookie.Name = "Admin";
    });
services.AddAuthorization();
services.AddDistributedMemoryCache();

services.AddSession(options =>
{
    // Set a short timeout for easy testing.
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    //options.Cookie.SecurePolicy = CookieSecurePolicy.Always; //require https
    // Make the session cookie essential
    options.Cookie.IsEssential = true;
});

services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => false; //true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
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
app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();
app.UseCookiePolicy();
app.UseSession();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapGet("/", context =>
    {
        return Task.Run(() => context.Response.Redirect("/admin/home"));
    });
});

app.Run();
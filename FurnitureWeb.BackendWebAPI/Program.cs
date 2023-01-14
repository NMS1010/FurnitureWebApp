using Domain.EF;
using Domain.Entities;
using FluentValidation;
using FurnitureWeb.Services.Catalog.Brands;
using FurnitureWeb.Services.Catalog.CartItems;
using FurnitureWeb.Services.Catalog.Categories;
using FurnitureWeb.Services.Catalog.Discounts;
using FurnitureWeb.Services.Catalog.OrderItemItems;
using FurnitureWeb.Services.Catalog.OrderItems;
using FurnitureWeb.Services.Catalog.Orders;
using FurnitureWeb.Services.Catalog.ProductImages;
using FurnitureWeb.Services.Catalog.Products;
using FurnitureWeb.Services.Catalog.ReviewItems;
using FurnitureWeb.Services.Catalog.WishItems;
using FurnitureWeb.Services.Common.FileStorage;
using FurnitureWeb.Services.External.MailJet;
using FurnitureWeb.Services.System.Roles;
using FurnitureWeb.Services.System.Users;
using FurnitureWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using System;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using FurnitureWeb.Utilities.Constants.Systems;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
// Add services to the container.
services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("AppDbContext")));

services.AddIdentity<AppUser, IdentityRole>(opts =>
{
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequiredLength = 5;
    opts.Password.RequireDigit = false;
    opts.Password.RequireLowercase = false;
    opts.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
services.AddScoped<IProductServices, ProductServices>();
services.AddScoped<IProductImageServices, ProductImageServices>();
services.AddScoped<ICategoryServices, CategoryServices>();
services.AddScoped<IBrandServices, BrandServices>();
services.AddScoped<IDiscountServices, DiscountServices>();
services.AddScoped<IReviewItemServices, ReviewItemServices>();
services.AddScoped<IOrderServices, OrderServices>();
services.AddScoped<IOrderItemServices, OrderItemServices>();
services.AddScoped<ICartItemServices, CartItemServices>();
services.AddScoped<IWishItemServices, WishItemServices>();

services.AddScoped<IFileStorageService, FileStorageService>();
services.AddScoped<IUserServices, UserServices>();
services.AddScoped<IRoleServices, RoleServices>();
services.AddScoped<IMailJetServices, MailJetServices>();

services.AddHttpContextAccessor();
services.AddHttpClient();
services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

services.AddSwaggerGen(s =>
{
    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = @"JWT authorization header using the Bearer sheme. \r\n\r\n
                        Enter 'Bearer' [space] and then your token in the text input below.
                        \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    s.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
});
string issuer = configuration.GetValue<string>("Tokens:Issuer");
string signingKey = configuration.GetValue<string>("Tokens:Key");
byte[] signingKeyBytes = Encoding.UTF8.GetBytes(signingKey);
services
    .AddAuthentication(opts =>
    {
        opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
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
    }).AddCookie(options =>
    {
        options.LoginPath = "/admin/login";
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
    }); ;
services.AddAuthorization();
services.AddSession();
services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

IServiceProvider serviceProvider = services.BuildServiceProvider();
async Task CreateRoles(IServiceProvider serviceProvider)
{
    var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    foreach (var roleName in SystemConstants.UserRoles.Roles.Values)
    {
        var roleExist = await RoleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            await RoleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}
await CreateRoles(serviceProvider);

var app = builder.Build();
var env = app.Environment;
// Configure the HTTP request pipeline.
if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseSwagger(c =>
{
    c.SerializeAsV2 = true;
});

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API for FurnitureWebApp V1");
});
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
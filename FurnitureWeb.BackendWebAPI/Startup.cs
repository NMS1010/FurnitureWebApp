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
using FurnitureWeb.Services.System.Users;
using FurnitureWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.BackendWebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("AppDbContext")));

            services.AddIdentity<AppUser, IdentityRole>()
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
            services.AddScoped<IWishListItemServices, WishListItemServices>();

            services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddScoped<IUserService, UserService>();

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
            string issuer = Configuration.GetValue<string>("Tokens:Issuer");
            string signingKey = Configuration.GetValue<string>("Tokens:Key");
            byte[] signingKeyBytes = Encoding.UTF8.GetBytes(signingKey);
            services
                .AddAuthentication(opts =>
                {
                    opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
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
                });
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
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
        }
    }
}
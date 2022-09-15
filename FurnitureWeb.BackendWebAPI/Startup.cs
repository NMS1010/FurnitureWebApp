using Domain.EF;
using FluentValidation;
using FluentValidation.AspNetCore;
using FurnitureWeb.Services.Catalog.Brands;
using FurnitureWeb.Services.Catalog.Categories;
using FurnitureWeb.Services.Catalog.Discounts;
using FurnitureWeb.Services.Catalog.Orders;
using FurnitureWeb.Services.Catalog.ProductImages;
using FurnitureWeb.Services.Catalog.Products;
using FurnitureWeb.Services.Catalog.Reviews;
using FurnitureWeb.Services.Common.FileStorage;
using FurnitureWeb.ViewModels.Catalog.Discounts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            services.AddScoped<IProductServices, ProductServices>();
            services.AddScoped<IProductImageServices, ProductImageServices>();
            services.AddScoped<ICategoryServices, CategoryServices>();
            services.AddScoped<IBrandServices, BrandServices>();
            services.AddScoped<IDiscountServices, DiscountServices>();
            services.AddScoped<IReviewServices, ReviewServices>();
            services.AddScoped<IOrderServices, OrderServices>();
            services.AddScoped<IFileStorageService, FileStorageService>();

            services.AddControllers()
                .AddFluentValidation(f => f.RegisterValidatorsFromAssemblyContaining<DiscountCreateRequestValidator>()); ;
            services.AddSwaggerGen();
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
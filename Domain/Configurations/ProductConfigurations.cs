using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Configurations
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");
            builder.HasKey(p => p.ProductId);
            builder
                .Property(p => p.ProductId)
                .UseIdentityColumn();
            builder
                .Property(p => p.Name)
                .HasMaxLength(255)
                .IsRequired();
            builder
                .Property(p => p.Description)
                .HasMaxLength(255)
                .IsRequired();
            builder
                .Property(p => p.Price)
                .HasColumnType("DECIMAL")
                .IsRequired();
            builder
                .Property(p => p.Quantity)
                .IsRequired();
            builder
                .Property(p => p.DateCreated)
                .IsRequired();
            builder
                .Property(p => p.Origin)
                .HasMaxLength(30)
                .IsRequired();
            builder
                .Property(p => p.Status)
                .IsRequired();

            builder
                .HasOne(x => x.Brand)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.BrandId);
            builder
                .HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId);
            builder
                .HasMany(p => p.Reviews)
                .WithOne(r => r.Product)
                .HasForeignKey(p => p.ProductId);

            builder
                .HasMany(p => p.ProductImages)
                .WithOne(pi => pi.Product)
                .HasForeignKey(p => p.ProductId);

            builder
                .HasMany(p => p.CartItems)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId);

            builder
                .HasMany(p => p.OrderItems)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId);
        }
    }
}
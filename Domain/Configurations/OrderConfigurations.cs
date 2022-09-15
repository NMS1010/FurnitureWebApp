using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Configurations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");

            builder.HasKey(x => x.OrderId);
            builder
                .Property(x => x.OrderId)
                .UseIdentityColumn();
            builder
                .Property(x => x.Shipping)
                .HasColumnType("DECIMAL")
                .IsRequired();
            builder
                .Property(x => x.TotalPrice)
                .HasColumnType("DECIMAL")
                .IsRequired();
            builder
                .Property(x => x.TotalItemPrice)
                .HasColumnType("DECIMAL")
                .IsRequired();
            builder.Property(x => x.Address)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(x => x.UserId)
                .IsRequired();
            builder
                .Property(x => x.DateCreated)
                .IsRequired();
            builder
                .Property(x => x.DateDone);
            builder
                .Property(x => x.Status)
                .IsRequired();
            builder
                .HasOne(x => x.Discount)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.DiscountId);
            builder
                .HasOne(x => x.User)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.UserId);
            builder
                .HasMany(x => x.OrderItems)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId);
        }
    }
}
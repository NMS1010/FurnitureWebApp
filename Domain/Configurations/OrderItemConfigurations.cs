﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Configurations
{
    public class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItem");

            builder
                .HasKey(x => x.OrderItemId);
            builder
                .Property(x => x.OrderItemId)
                .UseIdentityColumn();
            builder
                .Property(x => x.UnitPrice)
                .HasColumnType("DECIMAL")
                .IsRequired();
            builder
                .Property(x => x.TotalPrice)
                .HasColumnType("DECIMAL")
                .IsRequired();
            builder
                .Property(x => x.Quantity)
                .IsRequired();
            builder
                .Property(x => x.DateCreated)
                .IsRequired();
            builder
                .HasOne(x => x.Order)
                .WithMany(x => x.OrderItems)
                .HasForeignKey(x => x.OrderId);

            builder
                .HasOne(x => x.Product)
                .WithMany(x => x.OrderItems)
                .HasForeignKey(x => x.ProductId);
        }
    }
}
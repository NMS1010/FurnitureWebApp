﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Configurations
{
    public class CategoryConfigurations : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder
                .ToTable("Category");
            builder
                .HasKey(x => x.CategoryId);
            builder
                .Property(x => x.CategoryId)
                .UseIdentityColumn();

            builder
                .Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();
            builder
               .Property(x => x.Content)
               .HasMaxLength(255)
               .IsRequired(false);

            builder
                .HasMany(x => x.Products)
                .WithOne(x => x.Category)
                .HasForeignKey(x => x.CategoryId);
        }
    }
}
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Configurations
{
    public class BrandConfigurations : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.ToTable("Brand");

            builder.HasKey(x => x.BrandId);

            builder
                .Property(x => x.BrandId)
                .UseIdentityColumn();
            builder
                .Property(x => x.BrandName)
                .HasMaxLength(100)
                .IsRequired();
            builder
                .Property(x => x.Origin)
                .HasMaxLength(100)
                .IsRequired();
            builder
                .Property(x => x.ImagePath)
                .HasMaxLength(255)
                .IsRequired();
            builder
                .Property(x => x.ImageSize)
                .IsRequired();
            builder
                .HasMany(x => x.Products)
                .WithOne(x => x.Brand)
                .HasForeignKey(x => x.BrandId);
        }
    }
}
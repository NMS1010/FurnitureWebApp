using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Configurations
{
    public class ProductImageConfigurations : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("ProductImage");
            builder.HasKey(x => x.Id);
            builder
                .Property(x => x.Id)
                .UseIdentityColumn();
            builder
                .Property(x => x.Path)
                .IsRequired();
            builder
                .Property(x => x.ProductId)
                .IsRequired();
            builder
                .Property(x => x.IsDefault)
                .IsRequired();
            builder
                .HasOne(x => x.Product)
                .WithMany(x => x.ProductImages)
                .HasForeignKey(x => x.ProductId);
        }
    }
}
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Configurations
{
    public class WishListConfigurations : IEntityTypeConfiguration<WishList>
    {
        public void Configure(EntityTypeBuilder<WishList> builder)
        {
            builder.ToTable("WishList");
            builder.HasKey(x => x.WishListId);

            builder.Property(x => x.WishListId)
                .UseIdentityColumn();

            builder
                .Property(x => x.ProductId)
                .IsRequired();
            builder
                .Property(x => x.UserId)
                .IsRequired();
            builder
                .Property(x => x.Status)
                .IsRequired();
            builder
                .Property(x => x.DateAdded)
                .IsRequired();

            builder.HasOne(x => x.Product)
                .WithMany(x => x.WishLists)
                .HasForeignKey(x => x.ProductId);

            builder.HasOne(x => x.User)
                .WithMany(x => x.WishLists)
                .HasForeignKey(x => x.UserId);
        }
    }
}
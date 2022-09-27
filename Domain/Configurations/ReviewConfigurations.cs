using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Configurations
{
    public class ReviewConfigurations : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Review");

            builder
                .HasKey(x => x.ReviewId);
            builder
                .Property(x => x.ReviewId)
                .UseIdentityColumn();
            builder
                .Property(x => x.DateCreated)
                .IsRequired();
            builder
                .Property(x => x.UserId)
                .IsRequired();
            builder
                .Property(x => x.ProductId)
                .IsRequired();
            builder
                .Property(x => x.Status)
                .IsRequired();
            builder
                .Property(x => x.Rating)
                .IsRequired(false);
            builder
                .Property(x => x.Content)
                .HasMaxLength(255)
                .IsRequired(false);
            builder
                .HasOne(x => x.Product)
                .WithMany(x => x.Reviews)
                .HasForeignKey(x => x.ProductId);

            builder
                .HasOne(x => x.User)
                .WithMany(x => x.Reviews)
                .HasForeignKey(x => x.UserId);
        }
    }
}
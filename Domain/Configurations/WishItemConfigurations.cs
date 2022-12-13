using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configurations
{
    public class WishItemConfigurations : IEntityTypeConfiguration<WishItem>
    {
        public void Configure(EntityTypeBuilder<WishItem> builder)
        {
            builder.ToTable("WishListItem");
            builder.HasKey(x => x.WishItemId);

            builder.Property(x => x.WishItemId)
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
                .WithMany(x => x.WishItems)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
                .WithMany(x => x.WishItems)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
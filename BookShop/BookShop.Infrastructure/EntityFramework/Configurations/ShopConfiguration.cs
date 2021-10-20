using BookShop.Core.Entities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.EntityFramework.Configurations
{
    [UsedImplicitly]
    public class ShopConfiguration : IEntityTypeConfiguration<Shop>
    {
        public void Configure(EntityTypeBuilder<Shop> builder)
        {
            builder.ToTable(nameof(Shop), BookShopContext.DefaultSchemaName);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Sale).IsRequired();
            builder.Property(x => x.Balance).HasDefaultValue<double>(1000);
            builder.Property(x => x.MaxBookQuantity).HasDefaultValue<int>(10);

            builder.HasMany(x => x.Books)
                .WithOne(r => r.Shop)
                .HasForeignKey(t => t.ShopId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

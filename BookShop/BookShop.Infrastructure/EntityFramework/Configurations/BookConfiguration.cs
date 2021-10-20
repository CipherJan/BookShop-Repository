using BookShop.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.EntityFramework.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable(nameof(Book), BookShopContext.DefaultSchemaName);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Title).IsRequired();
            builder.Property(x => x.Author).IsRequired();
            builder.Property(x => x.Genre).IsRequired();
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.DiscountPrice).IsRequired();
            builder.Property(x => x.ReleaseDate).IsRequired();
            builder.Property(x => x.Novelty).IsRequired();
            builder.Property(x => x.ShopId);
        }
    }
}

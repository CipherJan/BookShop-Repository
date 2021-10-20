using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BookShop.Infrastructure.EntityFramework
{
    [UsedImplicitly]
    public sealed class BookShopContextContextDesignTimeFactory : IDesignTimeDbContextFactory<BookShopContext>
    {
        private const string DefaultConnectionString =
            "Data Source=127.0.0.1;Initial Catalog=BookShop; User Id=sa; Password=2wsx2WSX;";

        public static DbContextOptions<BookShopContext> GetSqlServerOptions([CanBeNull]string connectionString)
        {
            return new DbContextOptionsBuilder<BookShopContext>()
                .UseSqlServer(connectionString ?? DefaultConnectionString, x =>
                {
                    x.MigrationsHistoryTable("__EFMigrationsHistory", BookShopContext.DefaultSchemaName);
                })
                .Options;
        }
        public BookShopContext CreateDbContext(string[] args)
        {
            return new(GetSqlServerOptions(null));
        }
    }
}

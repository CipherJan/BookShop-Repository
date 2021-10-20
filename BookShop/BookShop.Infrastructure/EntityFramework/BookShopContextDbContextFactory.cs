namespace BookShop.Infrastructure.EntityFramework
{
    public sealed class BookShopContextDbContextFactory
    {
        private readonly string _connectionString;
        public BookShopContextDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }
        public BookShopContext GetContext()
        {
            return new(BookShopContextContextDesignTimeFactory.GetSqlServerOptions(_connectionString));
        }
    }
}

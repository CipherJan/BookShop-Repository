using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookShop.Core.Entities;

namespace BookShop.Infrastructure.EntityFramework
{
    public class BookShopContext : DbContext
    {
        public const string DefaultSchemaName = "BookShop";

        public BookShopContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            modelBuilder.HasDefaultSchema(DefaultSchemaName);
        }

        public async Task AddShop(Shop shop)
        {
            Add(shop);

            await SaveChangesAsync();
        }

        public async Task<IEnumerable<Shop>> GetShops()
        {
            return await Set<Shop>().Include(s => s.Books).ToListAsync();
        }

        public async Task<Shop> GetShop(int shopId)
        {
            return await Set<Shop>().Include(s => s.Books).SingleAsync(x => x.Id == shopId);
        }

        public async Task AddBookToShop(Book book, int shopId)
        {
            var shop = await Set<Shop>().Include(s => s.Books).SingleAsync(x => x.Id == shopId);

            shop.AddBook(book);

            await SaveChangesAsync();
        }

        public async Task AddSeveralBooksToShop(int shopId, double price, List<Book> books)
        {
            await using var transaction = await Database.BeginTransactionAsync();
            var shop = await Set<Shop>().Include(s => s.Books).SingleAsync(x => x.Id == shopId);

            shop.WithdrawMoney(price);
            shop.AddBooks(books);
            shop.SetMaxBookQuantity();

            await SaveChangesAsync();
            await transaction.CommitAsync();
        }

        public async Task<IEnumerable<Book>> GetAllBooksFromShop(int shopId)
        {
            var shop = await Set<Shop>().Include(s => s.Books).SingleAsync(x => x.Id == shopId);

            return shop.Books.Where(x => x.Status.Equals(BookSaleStatus.Asale));
        }

        public async Task<Book> GetBook(Guid bookId)
        {
            return await Set<Book>().SingleAsync(x => x.Id == bookId);
        }

        public async Task BuyBookFromShop(Guid bookId, int shopId)
        {
            await using var transaction = await Database.BeginTransactionAsync();
            var shop = await Set<Shop>().Include(x => x.Books).SingleAsync(x => x.Id == shopId);
            var book = shop.Books.Single(b => b.Id == bookId);

            if (shop.Sale.Equals(ShopSale.Active) && !book.IsNew())
            {
                shop.PutMoney(book.DiscountPrice);
            }
            else
            {
                shop.PutMoney(book.Price);
            }

            book.Sold();

            await SaveChangesAsync();
            await transaction.CommitAsync();
        }

        public async Task SetSaleStatusFromShop(int shopId, ShopSale sale)
        {
            var shop = await Set<Shop>().SingleAsync(x => x.Id == shopId);
            shop.ChangeSaleStatus(sale);
            await SaveChangesAsync();
        }

        public async Task<ShopSale> GetSaleStatusFromShop(int shopId)
        {
            var shop = await Set<Shop>().SingleAsync(x => x.Id == shopId);
            return shop.Sale;
        }

        public async Task Migrate()
        {
            await Database.MigrateAsync();
        }
    }
}
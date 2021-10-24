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

#warning унести в сервис
        private const double BookAcceptanceFeeInPercent = 0.07;
        private static double GetTotalPrice(double price) => BookAcceptanceFeeInPercent * price;

        public BookShopContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            modelBuilder.HasDefaultSchema(DefaultSchemaName);
        }

        public async Task AddBookInShop(Book book, int shopId)
        {
            var shop = await Set<Shop>().Include(s => s.Books).SingleAsync(x => x.Id == shopId);

            shop.AddBook(book);

            await SaveChangesAsync();
        }

        public async Task AddSeveralBooksInShop(int shopId, double price, List<Book> books)
        {
            var totalPrice = GetTotalPrice(price);

            await using var transaction = await Database.BeginTransactionAsync();
            var shop = await Set<Shop>().Include(s => s.Books).SingleAsync(x => x.Id == shopId);

            shop.WithdrawMoney(totalPrice);
            shop.AddBooks(books);
            shop.SetMaxBookQuantity();

            await SaveChangesAsync();
            await transaction.CommitAsync();
        }

        public async Task<Book> GetBook(Guid bookId)
        {
            return await Set<Book>().SingleAsync(x => x.Id == bookId);
        }

        public async Task<IEnumerable<Book>> GetAllBooksFromShop(int shopId)
        {
            var shop = await Set<Shop>().Include(s => s.Books).FirstOrDefaultAsync(x => x.Id == shopId);
            return shop.Books;
        }

        public async Task BuyBookFromShop(Guid bookId, int shopId)
        {
            await using var transaction = await Database.BeginTransactionAsync();
            var shop = await Set<Shop>().Include(x => x.Books).SingleAsync(x => x.Id == shopId);
            var book = shop.Books.Single(b => b.Id == bookId);

            if (shop.Sale.Equals(Sale.Active) && book.IsOld())
            {
                shop.PutMoney(book.DiscountPrice);
            }
            else
            {
                shop.PutMoney(book.Price);
            }

#warning добавить признак удалена ли книга, апдейтить его тут
            Remove(book);

            await SaveChangesAsync();
            await transaction.CommitAsync();
        }

        public async Task AddShop(Shop shop)
        {
            Add(shop);

            await SaveChangesAsync();
        }

        public async Task<Shop> GetShop(int shopId)
        {
            return await Set<Shop>().Include(s => s.Books).SingleAsync(x => x.Id == shopId);
        }

        public async Task<IEnumerable<Shop>> GetShops()
        {
            return await Set<Shop>().Include(s => s.Books).ToListAsync();
        }

        public async Task SetSaleStatusFromShop(int shopId, Sale sale)
        {
            var shop = await Set<Shop>().SingleAsync(x => x.Id == shopId);
            shop.ChangeSaleStatus(sale);
            await SaveChangesAsync();
        }

        public async Task<Sale> GetSaleStatusFromShop(int shopId)
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
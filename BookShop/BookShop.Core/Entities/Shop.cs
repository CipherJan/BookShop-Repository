using System;
using System.Collections.Generic;
using BookShop.Core.Models;
using BookShop.Core.Exceptions;

namespace BookShop.Core.Entities
{
    public class Shop
    {
        private static double MINIMUN_COUNT_OF_BOOKS_IN_PERCENT = 0.1;
        public int Id { get; private set; }
        public string Name { get; private set; }
        public Sale Sale { get; private set; }
        public double Balance { get; private set; }
        public int MaxBookQuantity { get; private set; }
        public List<Book> Books { get; private set; }

        private Shop()
        {

        }

        public Shop(ShopModel model)
        {
            Name = model.Name;
            Sale = Sale.Inactive;
            Balance = model.Balance;
            MaxBookQuantity = model.MaxBookQuantity;
            Books = new List<Book>();
        }

        public void AddBook(Book book)
        {
            Books.Add(book);
        }

        public void AddBooks(List<Book> books)
        {
            Books.AddRange(books);
        }

        public void PutMoney(double sum)
        {
            Balance += sum;
        }

        public void WithdrawMoney(double sum)
        {
            if (Balance >= sum)
            {
                Balance -= sum;
                return;
            }
            throw new InsufficientFundsOnBalanceException(
                message: $"Problem from {nameof(Shop)}. Insufficient funds on the balance.",
                shopId: Id,
                balanceInShop: Balance,
                writeOffAmount: sum);
        }

        public void SetMaxBookQuantity()
        {
            if (Books.Count > MaxBookQuantity)
            {
                MaxBookQuantity = Books.Count;
            }
        }

        public void ChangeSaleStatus(Sale sale)
        {
            Sale = sale;
        }
        public bool CheckYouNeedBooks() => MINIMUN_COUNT_OF_BOOKS_IN_PERCENT > GetNumberOfBooksAsPercentage();

        private double GetNumberOfBooksAsPercentage() => Math.Round((double)Books.Count / MaxBookQuantity, 2);

        public int GetNumberOfBooksToOrder() => MaxBookQuantity - Books.Count;
    }
}

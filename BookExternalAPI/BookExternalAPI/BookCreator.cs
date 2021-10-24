using System;
using System.Collections.Generic;
using Core;

namespace BookExternalAPI
{
    public static class BookCreator
    {
        private static readonly string[] BookGenres = { "Adventure", "Encyclopedia", "Fantastic", "Drama", "Novel", };
        private static readonly string[] AuthorFirstNames = { "Paul", "George", "Oscar", "Emma", "Monika" };
        private static readonly string[] AuthorLastNames = { "Show", "Blanche", "Redklif", "Wilde", "Bradbury", "Remarque", "Huxley" };
        private static Random _randomizer;
        
        public static List<Book> GetBooks(int numberOfBooks)
        {
            var books = new List<Book>();
            for (var i = 0; i < numberOfBooks; i++)
            {
                books.Add(CreatBook());
            }
            return books;
        }
        private static Book CreatBook()
        {
            _randomizer = new Random();
            var author = GetAuthor();
            var genre = GetGenre();
            var price = GetPrice();
            var releaseDate = GetReleaseDate();
            var title = GetTitle(author: author, genre: genre, releaseDate: releaseDate);

            return new Book()
            {
                Author = author,
                Genre = genre,
                Price = price,
                ReleaseDate = releaseDate,
                Title = title
            };
        }
        private static string GetAuthor() => $"{GetFirstName()} {GetLastName()}";

        private static string GetGenre() => BookGenres[_randomizer.Next(BookGenres.Length)];

        private static double GetPrice() => Math.Round((_randomizer.Next(150, 640)) / Math.PI, 2);

        private static DateTime GetReleaseDate()
        {
            var dateOffset = _randomizer.Next(10, 700);
            return DateTime.Now.AddDays(-dateOffset);
        }
        private static string GetTitle(string author, string genre, DateTime releaseDate) => $"{genre} of {author} released on {releaseDate.DayOfWeek} {releaseDate.Date:d}";

        private static string GetFirstName() => AuthorFirstNames[_randomizer.Next(AuthorFirstNames.Length)];

        private static string GetLastName() => AuthorLastNames[_randomizer.Next(AuthorLastNames.Length)];

    }
}

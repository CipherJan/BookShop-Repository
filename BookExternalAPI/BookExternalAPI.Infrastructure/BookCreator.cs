using System;
using System.Collections.Generic;
using Core;

namespace Infrastructure
{
    public static class BookCreator
    {
        private static readonly string[] _bookGenres = new[] { "Adventure", "Encyclopedia", "Fantastic", "Drama", "Novel", };
        private static readonly string[] _authorFirstNames = new[] { "Paul", "George", "Oscar", "Emma", "Monika" };
        private static readonly string[] _authorLastNames = new[] { "Show", "Blanche", "Redklif", "Wilde", "Bradbury", "Remarque", "Huxley" };
        private static Random randomizer;
        
        public static List<Book> GetBooks(int numberOfBooks)
        {
            var books = new List<Book>();
            for (int i = 0; i < numberOfBooks; i++)
            {
                books.Add(CreatBook());
            }
            return books;
        }
        private static Book CreatBook()
        {
            randomizer = new Random();
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

        private static string GetGenre() => _bookGenres[randomizer.Next(_bookGenres.Length)];

        private static double GetPrice() => Math.Round((randomizer.Next(150, 640)) / Math.PI, 2);

        private static DateTime GetReleaseDate()
        {
            var dateOffset = randomizer.Next(10, 700);
            return DateTime.Now.AddDays(-dateOffset);
        }
        private static string GetTitle(string author, string genre, DateTime releaseDate) => $"{genre} of {author} released on {releaseDate.DayOfWeek} {releaseDate.Date:d}";

        private static string GetFirstName() => _authorFirstNames[randomizer.Next(_authorFirstNames.Length)];

        private static string GetLastName() => _authorLastNames[randomizer.Next(_authorLastNames.Length)];

    }
}

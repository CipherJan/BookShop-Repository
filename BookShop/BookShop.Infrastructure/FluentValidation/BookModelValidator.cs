using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShop.Core.Entities;
using BookShop.Core.Models.BookModel;
using FluentValidation;

namespace BookShop.Infrastructure.FluentValidation
{
    public class BookModelValidator : AbstractValidator<BookModel>
    {
        private static readonly string _bookGenre; 
        static BookModelValidator()
        {
            foreach (var genre in Enum.GetValues<BookGenre>())
            {
                _bookGenre += $"{genre} ";
            }
        }
        public BookModelValidator()
        {
            RuleFor(t => t.Title)
                .NotNull()
                .NotEmpty()
                .WithMessage("The book must contain a Title");
            RuleFor(t => t.Author)
                .NotNull()
                .NotEmpty()
                .WithMessage("The book must contain a Author");
            RuleFor(t => t.Genre)
                .NotNull()
                .NotEmpty()
                .WithMessage("The book must contain a Genre")
                .IsEnumName(typeof(BookGenre))
                .WithMessage($"Genre must be one of {_bookGenre}");
            RuleFor(t => t.Price)
                .NotNull()
                .WithMessage("Price must be set")
                .GreaterThan(0)
                .WithMessage("Price must be greater then 0");
            RuleFor(t => t.ReleaseDate)
                .LessThanOrEqualTo(DateTime.Today)
                .WithMessage("ReleaseDate can't be from the future");
        }
    }
}

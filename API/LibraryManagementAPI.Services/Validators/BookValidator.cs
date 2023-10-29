using FluentValidation;
using LibraryManagementAPI.Services.Constants;
using LibraryManagementAPI.Services.Interfaces;
using LibraryManagementAPI.Services.Models;

namespace LibraryManagementAPI.Data.Validators
{
    public class BookValidator : AbstractValidator<BookDto>, IBookValidator
    {
        public BookValidator()
        {
            RuleFor(book => book.ISBN)
                .NotEmpty().WithMessage(BookErrors.ISBNRequired)
                .Matches(ValidationPatterns.IsbnPattern)
                .WithMessage(BookErrors.InvalidISBNFormat);

            RuleFor(book => book.Title)
                .NotEmpty().WithMessage(BookErrors.TitleRequired);

            RuleFor(book => book.Author)
                .NotEmpty().WithMessage(BookErrors.AuthorRequired);

            RuleFor(book => book.Genre)
                .NotEmpty().WithMessage(BookErrors.GenreRequired);
        }

        public void ValidateBook(BookDto bookDto)
        {
            var validationResult = Validate(bookDto);
            if (!validationResult.IsValid)
            {
                var validationErrors = validationResult.Errors.Select(error => error.ErrorMessage);
                throw new ValidationException(string.Join("\n", validationErrors));
            }
        }
    }
}
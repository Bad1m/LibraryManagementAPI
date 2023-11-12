namespace LibraryManagementAPI.Services.Constants
{
    public class BookErrors
    {
        public const string ISBNRequired = "ISBN is required.";
        public const string InvalidISBNFormat = "Invalid ISBN format.";
        public const string TitleRequired = "Title is required.";
        public const string AuthorRequired = "Author is required.";
        public const string GenreRequired = "Genre is required.";
        public const string ISBNAlreadyExists = "ISBN already exists.";
        public const string BookNotFound = "Book not found.";
        public const string FailedToDeleteBook = "Failed to delete the book.";
    }
}
using LibraryManagementAPI.Services.Models;

namespace LibraryManagementAPI.Services.Interfaces
{
    public interface IBookService
    {
        Task<BookDto> CreateBookAsync(BookDto book, CancellationToken cancellationToken);
        Task<List<BookDto>> GetAllBooksAsync(CancellationToken cancellationToken);
        Task<BookDto> GetBookAsync(int id, CancellationToken cancellationToken);
        Task<BookDto> GetBookByISBNAsync(string isbn, CancellationToken cancellationToken);
        Task<BookDto> UpdateBookAsync(int id, BookDto book, CancellationToken cancellationToken);
        Task DeleteBookAsync(int id, CancellationToken cancellationToken);
    }
}
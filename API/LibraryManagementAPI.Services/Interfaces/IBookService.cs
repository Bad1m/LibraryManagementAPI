using LibraryManagementAPI.Services.Models;

namespace LibraryManagementAPI.Services.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetBooksAsync();
        Task<BookDto> GetBookAsync(int id);
        Task<BookDto> GetBookByISBNAsync(string isbn);
        Task<BookDto> CreateBookAsync(BookDto book);
        Task<BookDto> UpdateBookAsync(int id, BookDto book);
        Task<bool> DeleteBookAsync(int id);
    }
}
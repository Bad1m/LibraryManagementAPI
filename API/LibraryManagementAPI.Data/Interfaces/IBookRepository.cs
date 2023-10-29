using LibraryManagementAPI.Data.Entities;

namespace LibraryManagementAPI.Data.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<Book?> GetByISBNAsync(string isbn);
    }
}

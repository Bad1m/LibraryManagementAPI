using LibraryManagementAPI.Data.Entities;
using LibraryManagementAPI.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace LibraryManagementAPI.Data.Repositories
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        public BookRepository(LibraryContext context) : base(context)
        {
        }

        public Task<Book?> GetByISBNAsync(string isbn)
        {
            return DbSet.AsNoTracking().FirstOrDefaultAsync(_ => _.ISBN == isbn);
        }
    }
}

using LibraryManagementAPI.Services.Models;

namespace LibraryManagementAPI.Services.Interfaces
{
    public interface IBookValidator
    {
        void ValidateBook(BookDto bookDto);
    }
}
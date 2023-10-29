using LibraryManagementAPI.Services.Models;

namespace LibraryManagementAPI.Services.Interfaces
{
    public interface IUserValidator
    {
        void ValidateUser(UserDto userDto);
    }
}

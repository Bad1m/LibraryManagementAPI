using LibraryManagementAPI.Services.Models;

namespace LibraryManagementAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<AuthenticateResponse> Register(UserDto userDto);
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto> UpdateUserAsync(int id, UserDto user);
        Task<bool> DeleteUserAsync(int id);
    }
}

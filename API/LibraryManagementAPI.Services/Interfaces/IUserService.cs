using LibraryManagementAPI.Services.Models;

namespace LibraryManagementAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<AuthenticateResponse> RegisterAsync(UserDto userDto, CancellationToken cancellationToken);
        Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model, CancellationToken cancellationToken);
        Task<UserDto?> GetUserByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken);
        Task<UserDto> UpdateUserAsync(int id, UserDto user, CancellationToken cancellationToken);
        Task DeleteUserAsync(int id, CancellationToken cancellationToken);
    }
}
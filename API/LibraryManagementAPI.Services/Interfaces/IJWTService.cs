using LibraryManagementAPI.Data.Entities;

namespace LibraryManagementAPI.Services.Interfaces
{
    public interface IJWTService
    {
        string GenerateJwtToken(User user);
    }
}
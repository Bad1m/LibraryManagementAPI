using LibraryManagementAPI.Data.Entities;

namespace LibraryManagementAPI.Data.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserByUsernameAsync(string username);
    }
}

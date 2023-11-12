using LibraryManagementAPI.Data.Entities;
using LibraryManagementAPI.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Data.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(LibraryContext context) : base(context)
        {
        }
        public Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            return DbSet.AsNoTracking().FirstOrDefaultAsync(_ => _.Username == username, cancellationToken);
        }
    }
}
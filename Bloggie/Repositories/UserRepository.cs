using Bloggie.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region Fields
        private readonly AuthDbContext _authDbContext;
        #endregion
        #region Constructors
        public UserRepository(AuthDbContext authDbContext)
        {
            _authDbContext = authDbContext;
        }
        #endregion
        public async Task<IEnumerable<IdentityUser>> GetAllUsersAsync()
        {
            var users = await _authDbContext.Users
                // .Select(q => q.Email != "superadmin@bloggie.com")
                .ToListAsync();

            var superAdminUser = await _authDbContext.Users
                .FirstOrDefaultAsync(q => q.Email == "superadmin@bloggie.com");

            if (superAdminUser is not null)
            {
                users.Remove(superAdminUser);
            }

            // Returns all users except Super Admin users.
            return users;
        }
        public async Task<IdentityUser> AddAsync(IdentityUser identityUser)
        {
            await _authDbContext.AddAsync(identityUser);
            await _authDbContext.SaveChangesAsync();
            return identityUser;
        }
    }
}

using Bloggie.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        public async Task<IEnumerable<IdentityUser>> GetAllUsersAsync(
             string? searchQuery = null,
            string? sortBy = null,
            string? sortDirection = null
            )
        {
            var users = await _authDbContext.Users
                .OrderBy(q => q.UserName)
                .ToListAsync();

            var superAdminUser = await _authDbContext.Users
                .FirstOrDefaultAsync(q => q.Email == "superadmin@bloggie.com");

            if (superAdminUser is not null)
            {
                users.Remove(superAdminUser);
            }

            var query = users.AsQueryable();

            // Search users
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query
                    .Where(
                    q => q.UserName.Contains(searchQuery) ||
                            q.Email.Contains(searchQuery)
                    );
            }

            // Sort users
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                var isDesc = string.Equals(sortDirection, "Desc", StringComparison.OrdinalIgnoreCase);

                // Sort by Username
                if (string.Equals(sortBy, "Username", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc
                        ?
                        query.OrderByDescending(q => q.UserName)
                        :
                        query.OrderBy(q => q.UserName);
                }
            }
            return query
                .ToList();

            // Code before sorting
            //var superAdminUser = await _authDbContext.Users
            // .FirstOrDefaultAsync(q => q.Email == "superadmin@bloggie.com");

            //if (superAdminUser is not null)
            //{
            //    users.Remove(superAdminUser);
            //}
            // Returns all users except Super Admin users.
            // return users;
        }
        public async Task<IdentityUser> AddAsync(IdentityUser identityUser)
        {
            await _authDbContext.AddAsync(identityUser);
            await _authDbContext.SaveChangesAsync();
            return identityUser;
        }
    }
}

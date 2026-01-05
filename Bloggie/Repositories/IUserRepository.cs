using Bloggie.Data;
using Microsoft.AspNetCore.Identity;

namespace Bloggie.Repositories
{
    public interface IUserRepository
    {
        public Task<IEnumerable<IdentityUser>> GetAllUsersAsync(
            string? searchQuery = null,
            string? sortBy = null,
            string? sortOrder = null
            );
        public Task<IdentityUser> AddAsync(IdentityUser identityUser);
    }
}

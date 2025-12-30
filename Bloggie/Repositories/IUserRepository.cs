using Bloggie.Data;
using Microsoft.AspNetCore.Identity;

namespace Bloggie.Repositories
{
    public interface IUserRepository
    {
        public Task<IEnumerable<IdentityUser>> GetAllUsersAsync();
        public Task<IdentityUser> AddAsync(IdentityUser identityUser);
    }
}

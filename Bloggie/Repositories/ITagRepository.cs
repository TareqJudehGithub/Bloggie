using Bloggie.Models.Domain;
using Bloggie.Models.ViewModel;

namespace Bloggie.Repositories
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAll(
           string? searchQuery = null,
           string? sortBy = null,
           string? sortDirection = null,
           int pageSize = 100,
           int pageNumber = 1
           );
        Task<Tag?> Get(Guid Id);
        Task<Tag> Add(Tag model);
        Task<Tag?> Edit(Tag model);
        Task<Tag?> Delete(Guid Id);

        // Pagination
        Task<int> CountAsync();
    }
}

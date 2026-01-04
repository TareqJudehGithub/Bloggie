using Bloggie.Models.Domain;
using Bloggie.Models.ViewModel;

namespace Bloggie.Repositories
{
    public interface ITagRepository
    {
        public Task<IEnumerable<Tag>> GetAll(
            string? searchString = null,
            string? sortBy = null,
            string? sortDirection = null
            );
        public Task<Tag?> Get(Guid Id);
        public Task<Tag> Add(Tag model);
        public Task<Tag?> Edit(Tag model);
        public Task<Tag?> Delete(Guid Id);
    }
}

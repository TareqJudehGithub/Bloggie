using Bloggie.Models.Domain;
using Bloggie.Models.ViewModel;

namespace Bloggie.Repositories
{
    public interface ITagRepository
    {
        public Task<IEnumerable<ReadOnlyRequestVM>> GetAll();
        public Task<ReadOnlyRequestVM?> Get(Guid Id);
        public Task<Tag> Add(AddTagRequestVM viewModel);
        public Task<Tag> Edit(EditTagRequestVM viewModel);
        public Task<Tag?> Delete(Guid Id);
    }
}

using Bloggie.Models.Domain;
using Bloggie.Models.ViewModel;

namespace Bloggie.Repositories
{
    public interface ITagRepository
    {
        public Task<IEnumerable<ReadOnlyTagRequestVM>> GetAll();
        public Task<Tag?> Get(Guid Id);
        public Task<Tag> Add(AddTagRequestVM viewModel);
        public Task<Tag> Edit(EditTagRequestVM viewModel);
        public Task<Tag?> Delete(Guid Id);
    }
}

using Bloggie.Models.Domain;
using Bloggie.Models.ViewModel;

namespace Bloggie.Repositories
{
    public interface IBlogPostRepository
    {
        public Task<IEnumerable<BlogPost>> GetAll();
        Task<BlogPost?> Get(Guid id);
        Task<BlogPost?> Add(BlogPost blogPost);
        Task<BlogPost> Update(BlogPost blogPost);
        Task<BlogPost> Delete(Guid id);

    }
}


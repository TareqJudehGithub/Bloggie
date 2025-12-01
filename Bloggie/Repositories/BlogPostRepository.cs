using Bloggie.Data;
using Bloggie.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        #region Fields
        private readonly BloggieDbContext _bloggieDbContext;
        #endregion

        #region Constructor
        public BlogPostRepository(BloggieDbContext bloggieDbContext)
        {
            _bloggieDbContext = bloggieDbContext;
        }
        #endregion

        #region Methods
        public async Task<IEnumerable<BlogPost>> GetAll()
        {
            var model = await _bloggieDbContext.BlogPosts.ToListAsync();
            return model;
        }
        public async Task<BlogPost> Add(BlogPost blogPost)
        {
            await _bloggieDbContext.BlogPosts.AddAsync(blogPost);
            await _bloggieDbContext.SaveChangesAsync();
            return blogPost;

        }
        #endregion
    }
}

using Bloggie.Data;
using Bloggie.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Repositories
{
    public class BlogPostLikesRepository : IBlogPostLikesRepository
    {
        #region Fields
        private readonly BloggieDbContext _bloggieDbContext;
        #endregion

        #region Constructor
        public BlogPostLikesRepository(BloggieDbContext bloggieDbContext)
        {
            _bloggieDbContext = bloggieDbContext;
        }
        #endregion
        #region Action Methods
        public async Task<int> GetTotalLikes(Guid blogPostId)
        {
            // count total counts of Likes
            return await _bloggieDbContext.Likes
                .CountAsync(q => q.BlogPostId == blogPostId);
        }
        public async Task<BlogPostLike> AddLikeForBlogPost(BlogPostLike blogPostLike)
        {
            await _bloggieDbContext.Likes.AddAsync(blogPostLike);
            await _bloggieDbContext.SaveChangesAsync();

            return blogPostLike;
        }
        #endregion
    }
}


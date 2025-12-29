using Bloggie.Data;
using Bloggie.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Repositories
{
    public class BlogPostCommentRepository : IBlogPostCommentRepository
    {
        #region Fields
        private readonly BloggieDbContext _bloggieDbContext;
        #endregion

        #region Constructor
        public BlogPostCommentRepository(BloggieDbContext bloggieDbContext)
        {
            _bloggieDbContext = bloggieDbContext;
        }
        #endregion

        #region Methods

        public async Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment)
        {
            await _bloggieDbContext.Comments.AddAsync(blogPostComment);
            await _bloggieDbContext.SaveChangesAsync();

            return blogPostComment;
        }
        public async Task<IEnumerable<BlogPostComment>> GetCommentsByBlogIdAsync(Guid blogPostId)
        {
            // Gets all comments for a blog post
            return await _bloggieDbContext.Comments
                .Where(q => q.BlogPostId == blogPostId)
                .ToListAsync();
        }
        #endregion
    }
}

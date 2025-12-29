using Bloggie.Models.Domain;

namespace Bloggie.Repositories
{
    public interface IBlogPostCommentRepository
    {
        Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment);
    }
}

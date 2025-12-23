using Bloggie.Models.Domain;

namespace Bloggie.Repositories
{
    public interface IBlogPostLikesRepository
    {
        Task<int> GetTotalLikes(Guid blogPostId);
        Task<BlogPostLike> AddLikeForBlogPost(BlogPostLike blogPostLike);
    }
}

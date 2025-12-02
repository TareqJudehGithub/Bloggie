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
        public async Task<BlogPost> Get(Guid id)
        {
            var blogPost = await _bloggieDbContext.BlogPosts.FirstOrDefaultAsync(q => q.Id == id);

            return blogPost;
        }
        public async Task<BlogPost> Add(BlogPost blogPost)
        {
            await _bloggieDbContext.BlogPosts.AddAsync(blogPost);
            await _bloggieDbContext.SaveChangesAsync();
            return blogPost;
        }
        public async Task<BlogPost> Update(BlogPost blogPost)
        {
            var existingPost = await _bloggieDbContext.BlogPosts
                 .FirstOrDefaultAsync(q => q.Id == blogPost.Id);

            if (existingPost != null)
            {
                existingPost.Heading = blogPost.Heading;
                existingPost.PageTitle = blogPost.PageTitle;
                existingPost.Content = blogPost.Content;
                existingPost.ShortDescription = blogPost.ShortDescription;
                existingPost.FeaturedImgUrl = blogPost.FeaturedImgUrl;
                existingPost.UrlHandle = blogPost.UrlHandle;
                existingPost.PublishedDate = blogPost.PublishedDate;
                existingPost.Author = blogPost.Author;
                existingPost.isVisible = blogPost.isVisible;
            }
            _bloggieDbContext.BlogPosts.Update(existingPost);
            await _bloggieDbContext.SaveChangesAsync();

            return existingPost;
        }
        public async Task<BlogPost> Delete(Guid id)
        {
            var blogPost = await _bloggieDbContext.BlogPosts.FirstOrDefaultAsync(q => q.Id == id);
            _bloggieDbContext.BlogPosts.Remove(blogPost);
            await _bloggieDbContext.SaveChangesAsync();
            return blogPost;
        }
        #endregion
    }
}

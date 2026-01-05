using Bloggie.Data;
using Bloggie.Models.Domain;
using Bloggie.Models.ViewModel;
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
        public async Task<IEnumerable<BlogPost>> GetAll(
            string? searchQuery = null,
            string? sortBy = null,
            string? sortDirection = null
            )
        {

            // Convert BlogPosts to a list we can query

            var query = _bloggieDbContext.BlogPosts.AsQueryable();
            query = query.OrderBy(q => q.Heading);

            // Search using Heading property
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(q => q.Heading.Contains(searchQuery));
            }

            // Sort Heading data
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                var isDecs = string.Equals(sortDirection, "Desc", StringComparison.OrdinalIgnoreCase);

                if (string.Equals(sortBy, "PageTitle", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDecs
                        ?
                        query.OrderByDescending(q => q.Heading)
                        :
                        query.OrderBy(q => q.Heading);
                }
            }

            return await query
               .Include(q => q.Tags)
               .ToListAsync();

            // Using .Include() here to bring from the database a related property (navigation property)
            // from the BlogPost Domain Model
            //var model = await _bloggieDbContext.BlogPosts
            //    .Include(q => q.Tags)
            //    .ToListAsync();
            //return model;
        }

        public async Task<BlogPost?> Get(Guid id)
        {
            // Using .Include() here to bring from the database a related property (navigation property)
            // from the BlogPost Domain Model           
            var blogPost = await _bloggieDbContext.BlogPosts
                .Include(u => u.Tags)
                .FirstOrDefaultAsync(q => q.Id == id);
            return blogPost;
        }
        public async Task<BlogPost> Add(BlogPost blogPost)
        {
            await _bloggieDbContext.BlogPosts.AddAsync(blogPost);
            await _bloggieDbContext.SaveChangesAsync();
            return blogPost;
        }
        public async Task<BlogPost?> Update(BlogPost blogPost)
        {
            // Include()  makes sure the Tags navigation property is included in the Update as well
            var existingBlog = await _bloggieDbContext.BlogPosts
                .Include(q => q.Tags)
                 .FirstOrDefaultAsync(q => q.Id == blogPost.Id);


            if (existingBlog != null)
            {
                // Update the record it self in the database
                existingBlog.Id = blogPost.Id;
                existingBlog.Heading = blogPost.Heading;
                existingBlog.PageTitle = blogPost.PageTitle;
                existingBlog.Content = blogPost.Content;
                existingBlog.ShortDescription = blogPost.ShortDescription;
                existingBlog.FeaturedImgUrl = blogPost.FeaturedImgUrl;
                existingBlog.UrlHandle = blogPost.UrlHandle;
                existingBlog.PublishedDate = blogPost.PublishedDate;
                existingBlog.Author = blogPost.Author;
                existingBlog.isVisible = blogPost.isVisible;
                existingBlog.Tags = blogPost.Tags;

                // Or: _bloggieDbContext.BlogPosts.Update(existingBlog);
                await _bloggieDbContext.SaveChangesAsync();

                return existingBlog;
            }
            return null;
        }
        public async Task<BlogPost?> Delete(Guid id)
        {
            var blogPost = await _bloggieDbContext.BlogPosts.FirstOrDefaultAsync(q => q.Id == id);

            if (blogPost != null)
            {
                _bloggieDbContext.BlogPosts.Remove(blogPost);
                await _bloggieDbContext.SaveChangesAsync();
                return blogPost;
            }
            return null;
        }
        public async Task<BlogPost?> GetByUrlHandle(string urlHandle)
        {
            return await _bloggieDbContext.BlogPosts
                // include Tags related to the founded blog post
                .Include(q => q.Tags)
                .FirstOrDefaultAsync(q => q.UrlHandle == urlHandle);
        }
        #endregion
    }
}

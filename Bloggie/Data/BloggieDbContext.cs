// Ignore Spelling: Bloggie

using Bloggie.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Data
{
    public class BloggieDbContext : DbContext
    {
        public BloggieDbContext(DbContextOptions<BloggieDbContext> options) : base(options)
        {

        }
        // Entities/Tables
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogPostLike> Likes { get; set; }
    }
}

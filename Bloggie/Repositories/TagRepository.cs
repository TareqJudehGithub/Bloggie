using Microsoft.EntityFrameworkCore;

using Bloggie.Models.Domain;
using Bloggie.Models.ViewModel;
using Bloggie.Data;

namespace Bloggie.Repositories
{
    public class TagRepository : ITagRepository
    {
        #region Fields
        private readonly BloggieDbContext _blogieDbContext;
        #endregion
        #region Constructor
        // Constructor injection
        public TagRepository(BloggieDbContext bloggieDbContext)
        {
            _blogieDbContext = bloggieDbContext;
        }
        #endregion

        #region methods  
        public async Task<IEnumerable<Tag>> GetAll(string? searchQuery)
        {

            // var domainModel = await _blogieDbContext.Tags.ToListAsync();
            // return domainModel;


            // Turn Tags to a list of items that we can query - Search
            var query = _blogieDbContext.Tags.AsQueryable();

            // Filtering
            if (string.IsNullOrWhiteSpace(searchQuery) == false)
            {
                // If the search input value matches any of Name of DisplayName, then return that  result back.
                query = query.Where(q => q.Name.Contains(searchQuery) ||
                q.DisplayName.Contains(searchQuery)

                );
            }
            // Sorting 

            // Pagination
            return await query.ToListAsync();

        }
        public async Task<Tag?> Get(Guid id)
        {

            var tag = await _blogieDbContext.Tags.FirstOrDefaultAsync(q => q.Id == id);
            return tag;
        }
        public async Task<Tag> Add(Tag tag)
        {

            await _blogieDbContext.Tags.AddAsync(tag);
            await _blogieDbContext.SaveChangesAsync();

            return tag;
        }
        public async Task<Tag?> Edit(Tag tag)
        {
            // Fetch the record to be updated
            var existingTag = await _blogieDbContext.Tags.FirstOrDefaultAsync(q => q.Id == tag.Id);

            // Check for null and update
            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                // Or _dbContext.Tags.Update(existingTag);

                // And save to the database
                await _blogieDbContext.SaveChangesAsync();

                // Back to the controller now
                return existingTag;
            }
            return null;
        }

        public async Task<Tag?> Delete(Guid Id)
        {
            var tag = await _blogieDbContext.Tags.FirstOrDefaultAsync(q => q.Id == Id);

            if (tag != null)
            {
                _blogieDbContext.Remove(tag);
                await _blogieDbContext.SaveChangesAsync();
                return tag;
            }

            return null;
        }
        #endregion
    }
}


/* 
 *  public Task<IEnumerable<Tag>> GetAllAsync();
        public Task<Tag?> Get(Guid Id);
        public Task<Tag> Add(Tag tag);
        public Task<Tag?> Update(Tag tag);
        public Task<Tag?> Delete(Guid Id);
 */

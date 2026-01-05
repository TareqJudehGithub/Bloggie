using Microsoft.EntityFrameworkCore;

using Bloggie.Models.Domain;
using Bloggie.Models.ViewModel;
using Bloggie.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Bloggie.Repositories
{
    public class TagRepository : ITagRepository
    {
        #region Fields
        private readonly BloggieDbContext _bloggieDbContext;
        #endregion
        #region Constructor
        // Constructor injection
        public TagRepository(BloggieDbContext bloggieDbContext)
        {
            _bloggieDbContext = bloggieDbContext;
        }
        #endregion

        #region methods  
        public async Task<IEnumerable<Tag>> GetAll(
            string? searchQuery,
            string? sortBy,
            string sortDirection
            )
        {
            // Turn Tags to a list of items that we can query - Search
            var query = _bloggieDbContext.Tags
                .OrderBy(q => q.Name)
                .AsQueryable();

            // query = query.OrderBy(q => q.Name);

            // Filtering
            // Use input value in searchQuery 
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                // If the search input value matches any of Name of DisplayName, then return that  result back.
                query = query
                    .Where(
                    q => q.Name.Contains(searchQuery) ||
                    q.DisplayName.Contains(searchQuery)
                );
            }

            // Sorting 
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                // Desc order - Check if sortDirection value = "Desc"
                var isDesc = string.Equals(sortDirection, "Desc", StringComparison.OrdinalIgnoreCase);

                var isAsc = string.Equals(sortDirection, "Asc", StringComparison.OrdinalIgnoreCase);

                // Check if sorting was by Name/Tag Code column
                if (string.Equals(sortBy, "Name", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc
                        ?
                        query.OrderByDescending(q => q.Name)
                        : query.OrderBy(q => q.Name);
                }

                // Check if sorting was by DisplayName/Tag Description column
                if (string.Equals(sortBy, "DisplayName", StringComparison.OrdinalIgnoreCase))
                {
                    if (isDesc)
                    {
                        query = query.OrderByDescending(q => q.DisplayName);
                        sortDirection = "Asc";
                    }
                    else
                    {
                        query = query.OrderBy(q => q.DisplayName);
                        sortDirection = "Desc";
                    }
                }
            }

            // Pagination

            return await query.ToListAsync();


            // Code before implementing Search and Sort
            // var domainModel = await _blogieDbContext.Tags.ToListAsync();
            // return domainModel;
        }
        public async Task<Tag?> Get(Guid id)
        {

            var tag = await _bloggieDbContext.Tags.FirstOrDefaultAsync(q => q.Id == id);
            return tag;
        }
        public async Task<Tag> Add(Tag tag)
        {

            await _bloggieDbContext.Tags.AddAsync(tag);
            await _bloggieDbContext.SaveChangesAsync();

            return tag;
        }
        public async Task<Tag?> Edit(Tag tag)
        {
            // Fetch the record to be updated
            var existingTag = await _bloggieDbContext.Tags.FirstOrDefaultAsync(q => q.Id == tag.Id);

            // Check for null and update
            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                // Or _dbContext.Tags.Update(existingTag);

                // And save to the database
                await _bloggieDbContext.SaveChangesAsync();

                // Back to the controller now
                return existingTag;
            }
            return null;
        }

        public async Task<Tag?> Delete(Guid Id)
        {
            var tag = await _bloggieDbContext.Tags.FirstOrDefaultAsync(q => q.Id == Id);

            if (tag != null)
            {
                _bloggieDbContext.Remove(tag);
                await _bloggieDbContext.SaveChangesAsync();
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

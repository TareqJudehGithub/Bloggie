using Microsoft.EntityFrameworkCore;

using Bloggie.Models.Domain;
using Bloggie.Models.ViewModel;
using Bloggie.Data;

namespace Bloggie.Repositories
{
    public class TagRepository : ITagRepository
    {
        #region Fields
        private readonly BloggieDbContext _dbContext;
        #endregion
        #region Constructor
        // Constructor injection
        public TagRepository(BloggieDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region methods  
        public async Task<IEnumerable<Tag>> GetAll()
        {
            var domainModel = await _dbContext.Tags.ToListAsync();


            return domainModel;
        }
        public async Task<Tag?> Get(Guid id)
        {

            var tag = await _dbContext.Tags.FirstOrDefaultAsync(q => q.Id == id);
            return tag;
        }
        public async Task<Tag> Add(Tag tag)
        {

            await _dbContext.Tags.AddAsync(tag);
            await _dbContext.SaveChangesAsync();

            return tag;
        }
        public async Task<Tag?> Edit(Tag tag)
        {
            // Fetch the record to be updated
            var existingTag = await _dbContext.Tags.FirstOrDefaultAsync(q => q.Id == tag.Id);

            // Check for null and update
            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                // Or _dbContext.Tags.Update(existingTag);

                // And save to the database
                await _dbContext.SaveChangesAsync();

                // Back to the controller now
                return existingTag;
            }
            return null;
        }

        public async Task<Tag?> Delete(Guid Id)
        {
            var tag = await _dbContext.Tags.FirstOrDefaultAsync(q => q.Id == Id);

            if (tag != null)
            {
                _dbContext.Remove(tag);
                await _dbContext.SaveChangesAsync();
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

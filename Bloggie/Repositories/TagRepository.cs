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
        public async Task<IEnumerable<ReadOnlyTagRequestVM>> GetAll()
        {
            var data = await _dbContext.Tags.ToListAsync();

            // Convert to View Model data
            var viewData = data.Select(q => new ReadOnlyTagRequestVM
            {
                Id = q.Id,
                Name = q.Name,
                DisplayName = q.DisplayName
            });
            return viewData;
        }
        public async Task<Tag> Get(Guid id)
        {

            var tag = await _dbContext.Tags.FirstOrDefaultAsync(q => q.Id == id);
            // Convert/map Model to View Model
            var viewData = new ReadOnlyTagRequestVM
            {
                Id = tag.Id,
                Name = tag.Name,
                DisplayName = tag.DisplayName
            };

            return tag;
        }
        public async Task<Tag> Add(AddTagRequestVM viewModel)
        {
            // 1. 'manual way' of model binding Model data, mapping AddTagRequestVM to Tag domain model       
            var model = new Tag
            {
                //  Id = viewModel.Id,
                Name = viewModel.Name,
                DisplayName = viewModel.DisplayName
            };
            // Adding the newly created record and saving into the database
            await _dbContext.Tags.AddAsync(model);
            await _dbContext.SaveChangesAsync();

            return model;
        }
        public async Task<Tag> Edit(EditTagRequestVM viewModel)
        {
            var existingTag = await _dbContext.Tags.FirstOrDefaultAsync(q => q.Id == viewModel.Id);

            if (existingTag != null)
            {
                existingTag.Name = viewModel.Name;
                existingTag.DisplayName = viewModel.DisplayName;

                _dbContext.Tags.Update(existingTag);
                await _dbContext.SaveChangesAsync();

                return existingTag;
            }
            return null;
        }

        public async Task<Tag> Delete(Guid Id)
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

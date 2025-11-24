// Ignore Spelling: bloggie

using Bloggie.Data;
using Bloggie.Models.Domain;
using Bloggie.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Controllers
{
    public class AdminTagsController : Controller
    {
        #region Fields
        private readonly BloggieDbContext _dbContext;
        #endregion

        #region Constructor
        public AdminTagsController(BloggieDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Action Methods
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _dbContext.Tags.ToListAsync();

            return View(model);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddTagRequestVM addTagRequest)
        {
            // 1. 'manual way' of model binding Model data, mapping AddTagRequestVM to Tag domain model       
            var model = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };

            // Saving to the database
            await _dbContext.Tags.AddAsync(model);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var tag = await _dbContext.Tags.FirstOrDefaultAsync(q => q.Id == Id);

            // Convert to View Model - EditTagRequest
            if (ModelState.IsValid)
            {
                var viewData = new EditTagRequestVM
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };

                return View(viewData);
            }
            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Guid Id, EditTagRequestVM editTagRequest)
        {
            // Convert back to Tag model
            if (Id == editTagRequest.Id)
            {
                var tag = new Tag
                {
                    Id = editTagRequest.Id,
                    Name = editTagRequest.Name,
                    DisplayName = editTagRequest.DisplayName
                };

                _dbContext.Update(tag);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Return to Edit screen in case Id was not found
            return RedirectToAction(nameof(Edit));
        }


        [HttpGet]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var tag = await _dbContext.Tags.FirstOrDefaultAsync(q => q.Id == Id);
            return View(tag);
        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid Id)
        {
            var tag = await _dbContext.Tags.FirstOrDefaultAsync(q => q.Id == Id);
            _dbContext.Tags.Remove(tag);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
    #endregion
}



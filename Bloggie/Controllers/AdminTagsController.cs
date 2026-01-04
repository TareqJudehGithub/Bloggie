// Ignore Spelling: bloggie

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Bloggie.Models.Domain;
using Bloggie.Models.ViewModel;
using Bloggie.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace Bloggie.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTagsController : Controller
    {
        #region Fields
        private readonly ITagRepository _tagRepository;
        #endregion

        #region Constructor
        public AdminTagsController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }
        #endregion

        #region Action Methods
        [HttpGet]
        public async Task<IActionResult> Index(
            string? searchQuery,
            string? sortBy,
            string? sortDirection
            )
        {

            // Save search state after submission a search request
            ViewBag.SearchQuery = searchQuery;

            // Sorting 
            ViewBag.SortBy = sortBy;
            ViewBag.SortDirection = sortDirection;

            var domainModel = await _tagRepository
                .GetAll(searchQuery, sortBy, sortDirection);

            // Convert to View Model data
            var viewData = domainModel.Select(q => new ReadOnlyTagRequestVM
            {
                Id = q.Id,
                Name = q.Name,
                DisplayName = q.DisplayName
            });

            return View(viewData);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddTagRequestVM addTagRequest)
        {
            // Adding a custom validation
            ValidateAddTagRequest(addTagRequest);

            if (ModelState.IsValid)
            {
                var model = new Tag
                {
                    Name = addTagRequest.Name,
                    DisplayName = addTagRequest.DisplayName
                };
                await _tagRepository.Add(model);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await _tagRepository.Get(id);

            // Convert to View Model - EditTagRequest
            if (model != null)
            {
                var viewData = new EditTagRequestVM
                {
                    Id = model.Id,
                    Name = model.Name,
                    DisplayName = model.DisplayName
                };

                return View(viewData);
            }
            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequestVM editTagRequest)
        {
            // Convert back to Domain model
            var domainModel = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };
            await _tagRepository.Edit(domainModel);

            if (ModelState.IsValid)
            {
                TempData["AlertType"] = "info";
                TempData["AlertMessage"] = "Tag was successfully updated!";

                return RedirectToAction(nameof(Index));
            }
            TempData["AlertType"] = "danger";
            TempData["AlertMessage"] = "Tag not found error!";
            return RedirectToAction(nameof(Add));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var model = await _tagRepository.Get(id);

            // Map Domain Model to View Model
            if (model != null)
            {
                var viewData = new ReadOnlyTagRequestVM
                {
                    Id = model.Id,
                    Name = model.Name,
                    DisplayName = model.DisplayName
                };

                return View(viewData);
            }

            TempData["AlertType"] = "danger";
            TempData["AlertMessage"] = "Error deleting tag!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid Id)
        {
            if (ModelState.IsValid)
            {
                TempData["AlertType"] = "info";
                TempData["AlertMessage"] = "Tag deleted successfully!";
                await _tagRepository.Delete(Id);
                return RedirectToAction(nameof(Index));
            }
            TempData["AlertType"] = "danger";
            TempData["AlertMessage"] = "Error deleting tag!";
            return RedirectToAction(nameof(Index));
        }


        // Custom Validation methods
        private void ValidateAddTagRequest(AddTagRequestVM addTagRequest)
        {
            if (addTagRequest.Name is not null && addTagRequest.DisplayName is not null)
            {
                if (addTagRequest.Name == addTagRequest.DisplayName)
                {
                    ModelState.AddModelError(
                        key: "DisplayName",
                        errorMessage: "Tag Code cannot be the same as Tag Description.");
                }
            }
        }

    }
    #endregion
}


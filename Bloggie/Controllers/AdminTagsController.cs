// Ignore Spelling: bloggie

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Bloggie.Models.Domain;
using Bloggie.Models.ViewModel;
using Bloggie.Repositories;


namespace Bloggie.Controllers
{
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
        public async Task<IActionResult> Index()
        {
            var domainModel = await _tagRepository.GetAll();
            // Convert to View Model data
            var viewData = domainModel.Select(q => new ReadOnlyTagRequestVM
            {
                Id = q.Id,
                Name = q.Name,
                DisplayName = q.DisplayName
            });

            return View(viewData);
        }


        [Authorize, HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddTagRequestVM addTagRequest)
        {
            var model = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };
            await _tagRepository.Add(model);
            return RedirectToAction(nameof(Index));
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
    }
    #endregion
}



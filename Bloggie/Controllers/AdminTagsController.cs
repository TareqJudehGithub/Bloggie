// Ignore Spelling: bloggie

using Bloggie.Data;
using Bloggie.Models.Domain;
using Bloggie.Models.ViewModel;
using Bloggie.Repositories;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var viewModel = await _tagRepository.GetAll();

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddTagRequestVM addTagRequest)
        {
            await _tagRepository.Add(addTagRequest);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var viewModel = await _tagRepository.Get(id);

            // Convert to View Model - EditTagRequest
            if (viewModel != null)
            {
                var viewData = new EditTagRequestVM
                {
                    Id = viewModel.Id,
                    Name = viewModel.Name,
                    DisplayName = viewModel.DisplayName
                };

                return View(viewData);
            }
            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequestVM editTagRequest)
        {
            await _tagRepository.Edit(editTagRequest);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var viewModel = await _tagRepository.Get(id);

            return View(viewModel);
        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid Id)
        {
            await _tagRepository.Delete(Id);
            return RedirectToAction(nameof(Index));
        }
    }
    #endregion
}



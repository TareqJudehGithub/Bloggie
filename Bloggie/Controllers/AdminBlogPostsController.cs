using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using Bloggie.Models.ViewModel;
using AspNetCoreGeneratedDocument;
using Bloggie.Repositories;

namespace Bloggie.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        #region Fields
        private readonly ITagRepository _tagRepository;
        #endregion

        #region Constructor
        public AdminBlogPostsController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }
        #endregion

        #region Methods
        // Get all tags
        public async Task<IActionResult> Index()
        {
            var posts = await _tagRepository.GetAll();
            return View(posts);
        }

        // Add -with select tag dropdown menu
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var tags = await _tagRepository.GetAll();

            var viewModel = new AddBlogPostRequestVM
            {
                Tags = tags.Select(q =>
                new SelectListItem
                {
                    Text = q.DisplayName,
                    Value = q.Id.ToString()
                })
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequestVM addVM)
        {
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}

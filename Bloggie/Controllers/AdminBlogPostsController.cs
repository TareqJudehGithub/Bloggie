using Microsoft.AspNetCore.Mvc;

using Bloggie.Models.ViewModel;
using AspNetCoreGeneratedDocument;

namespace Bloggie.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        public IActionResult Index(ReadOnlyBlogPostRequestVM viewModel)
        {

            return View();
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(AddBlogPostRequestVM viewModel)
        {
            return View();
        }
    }
}

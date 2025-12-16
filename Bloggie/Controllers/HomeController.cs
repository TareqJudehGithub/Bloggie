using System.Diagnostics;
using Bloggie.Models;
using Bloggie.Models.ViewModel;
using Bloggie.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ITagRepository _tagRepository;

        public HomeController(
            ILogger<HomeController> logger,
            IBlogPostRepository blogPostRepository,
            ITagRepository tagRepository
            )
        {
            _logger = logger;
            _blogPostRepository = blogPostRepository;
            _tagRepository = tagRepository;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _blogPostRepository.GetAll();
            var tags = await _tagRepository.GetAll();

            var viewData = model.Select(q => new ReadOnlyBlogPostRequestVM
            {
                Id = q.Id,
                Heading = q.Heading,
                PageTitle = q.PageTitle,
                Content = q.Content,
                ShortDescription = q.ShortDescription,
                FeaturedImgUrl = q.FeaturedImgUrl,
                UrlHandle = q.UrlHandle,
                PublishedDate = q.PublishedDate,
                Author = q.Author,
                isVisible = q.isVisible,
                Tags = q.Tags
            });
            if (viewData.Count() == 0)
            {
                TempData["AlertType"] = "secondary";
                TempData["AlertMessage"] = "No blogs were found! Try posting some :)";
            }

            var viewModel = new HomeViewModel
            {
                BlogPosts = viewData,
                Tags = tags
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

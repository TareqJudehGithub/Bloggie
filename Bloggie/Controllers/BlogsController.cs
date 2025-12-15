using Bloggie.Models.ViewModel;
using Bloggie.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Bloggie.Controllers
{
    public class BlogsController : Controller
    {
        #region Fields
        private readonly IBlogPostRepository _blogPostRepository;
        #endregion
        #region Constructor
        public BlogsController(IBlogPostRepository blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
        }
        #endregion
        #region Action Methods
        public async Task<IActionResult> Index(string urlHandle)
        {
            var modelData = await _blogPostRepository.GetByUrlHandle(urlHandle);

            if (modelData != null)
            {
                var viewData = new ReadOnlyBlogPostRequestVM
                {
                    Id = modelData.Id,
                    Heading = modelData.Heading,
                    PageTitle = modelData.PageTitle,
                    Content = modelData.Content,
                    ShortDescription = modelData.ShortDescription,
                    FeaturedImgUrl = modelData.FeaturedImgUrl,
                    UrlHandle = modelData.UrlHandle,
                    PublishedDate = modelData.PublishedDate,
                    Author = modelData.Author,
                    isVisible = modelData.isVisible,
                    Tags = modelData.Tags
                };
                return View(viewData);
            }
            TempData["AlertType"] = "danger";
            TempData["AlertMessage"] = "Blog was not found! :(";

            return null;
        }
        #endregion
    }
}

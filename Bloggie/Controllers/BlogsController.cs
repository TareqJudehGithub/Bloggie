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
        private readonly IBlogPostLikesRepository _blogPostLikesRepository;
        #endregion

        #region Constructor
        public BlogsController(
            IBlogPostRepository blogPostRepository,
            IBlogPostLikesRepository blogPostLikesRepository
            )
        {
            _blogPostRepository = blogPostRepository;
            _blogPostLikesRepository = blogPostLikesRepository;
        }
        #endregion

        #region Action Methods
        public async Task<IActionResult> Index(string urlHandle)
        {
            var modelData = await _blogPostRepository.GetByUrlHandle(urlHandle);
            var viewData = new ReadOnlyBlogPostDetailsVM();

            if (modelData != null)
            {
                // Total likes
                var totalLikes = await _blogPostLikesRepository.GetTotalLikes(modelData.Id);

                viewData = new ReadOnlyBlogPostDetailsVM
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
                    Tags = modelData.Tags,
                    TotalLikes = totalLikes
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

using Bloggie.Models.Domain;
using Bloggie.Models.ViewModel;
using Bloggie.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Controllers
{
    public class BlogsController : Controller
    {
        #region Fields
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IBlogPostLikesRepository _blogPostLikesRepository;
        private readonly IBlogPostCommentRepository _blogPostCommentRepository;

        #endregion

        #region Constructor
        public BlogsController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IBlogPostRepository blogPostRepository,
            IBlogPostLikesRepository blogPostLikesRepository,
            IBlogPostCommentRepository blogPostCommentRepository
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _blogPostRepository = blogPostRepository;
            _blogPostLikesRepository = blogPostLikesRepository;
            _blogPostCommentRepository = blogPostCommentRepository;
        }
        #endregion

        #region Action Methods
        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {

            var modelData = await _blogPostRepository.GetByUrlHandle(urlHandle);
            var viewData = new ReadOnlyBlogPostDetailsVM();
            var blogLiked = false;

            if (modelData != null)
            {
                // Total likes
                var totalLikes = await _blogPostLikesRepository.GetTotalLikes(modelData.Id);

                if (_signInManager.IsSignedIn(User))
                {
                    // a blog total likes
                    var blogTotalLikes = await _blogPostLikesRepository.GetLikesForBlog(modelData.Id);

                    var userId = _userManager.GetUserId(User);

                    // Check if the user already likes a post or not using that user Id
                    if (userId != null)
                    {
                        var likeFromUser = blogTotalLikes.FirstOrDefault(q => q.UserId == Guid.Parse(userId));
                        blogLiked = likeFromUser != null;

                    }
                }
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
                    TotalLikes = totalLikes,
                    blogLiked = blogLiked
                };
                return View(viewData);

            }
            TempData["AlertType"] = "danger";
            TempData["AlertMessage"] = "Blog was not found! :(";

            return null;
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> Index(ReadOnlyBlogPostDetailsVM viewModel)
        {
            if (_signInManager.IsSignedIn(User))
            {
                // Convert view model to domain model
                var model = new BlogPostComment
                {
                    BlogPostId = viewModel.Id,
                    Description = viewModel.CommentDescription,
                    UserId = Guid.Parse(_userManager.GetUserId(User)),
                    DateAdded = DateTime.Now
                };
                await _blogPostCommentRepository.AddAsync(model);

                return RedirectToAction(
                    controllerName: "Home",
                    actionName: nameof(Index));
            }
            else
            {
                TempData["AlertType"] = "warning";
                TempData["AlertMessage"] = "Please log in first to your account.";
                return Forbid();
            }


        }
        #endregion
    }
}

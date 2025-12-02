using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using Bloggie.Models.ViewModel;
using AspNetCoreGeneratedDocument;
using Bloggie.Repositories;
using Bloggie.Models.Domain;

namespace Bloggie.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        #region Fields
        private readonly ITagRepository _tagRepository;
        private readonly IBlogPostRepository _blogPostRepository;
        #endregion

        #region Constructor
        public AdminBlogPostsController(
            IBlogPostRepository blogPostRepository,
            ITagRepository tagRepository
            )
        {
            _tagRepository = tagRepository;
            _blogPostRepository = blogPostRepository;
        }
        #endregion

        #region Methods
        // Get all tags
        public async Task<IActionResult> Index()
        {
            var model = await _blogPostRepository.GetAll();
            return View(model);
        }

        // Add -with select tag drop-down menu
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
        public async Task<IActionResult> Add(AddBlogPostRequestVM viewModel)
        {
            // Map View Model => Domain Model
            var model = new BlogPost
            {
                Heading = viewModel.Heading,
                PageTitle = viewModel.PageTitle,
                Content = viewModel.Content,
                ShortDescription = viewModel.ShortDescription,
                FeaturedImgUrl = viewModel.FeaturedImgUrl,
                UrlHandle = viewModel.UrlHandle,
                PublishedDate = viewModel.PublishedDate,
                Author = viewModel.Author,
                isVisible = viewModel.isVisible
            };

            // Multiple tags selection from Tags drop-down list
            // Map selectedTags View Model => Domain Model
            var selectedTags = new List<Tag>();

            // Loop through the ids, and get the value from the database
            foreach (var selectedTagId in viewModel.SelectedTags)
            {
                // Convert Id (string) to Guid format
                var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
                // Try fetch the Id from the database
                var existingTag = await _tagRepository.Get(selectedTagIdAsGuid);

                // Assign that tag found to the Tags property in the BlogPost
                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }
                // Mapping tags back to domain model
                model.Tags = selectedTags;
            }

            // Binding one single choice from Tag drop-down list code:
            //var selectedTag = new List<Tag>();
            //var selectedTagGuidId = Guid.Parse(viewModel.SelectedTag);
            //var existingCurrentTag = await _tagRepository.Get(selectedTagGuidId);

            //if (existingCurrentTag != null)
            //{
            //    selectedTag.Add(existingCurrentTag);
            //}
            //model.Tags = selectedTag;

            await _blogPostRepository.Add(blogPost: model);

            return RedirectToAction(actionName: nameof(Add));
        }
        #endregion
    }
}

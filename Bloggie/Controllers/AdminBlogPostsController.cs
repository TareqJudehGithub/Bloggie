using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using Bloggie.Models.ViewModel;
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
            return View(viewData);
        }


        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var tags = await _tagRepository.GetAll();

            // Add -with select tag drop-down menu
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

            return RedirectToAction(actionName: nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            // Retrieve item from the database through the repository
            var model = await _blogPostRepository.Get(id);

            // Fetch all tags from the database
            var tagsDomainModel = await _tagRepository.GetAll();

            // Convert/map from the Domain Model to the View Model, and model-bind it to the view
            if (model != null)
            {
                var viewModel = new EditBlogPostRequestVM
                {
                    Id = model.Id,
                    Heading = model.Heading,
                    PageTitle = model.PageTitle,
                    Content = model.Content,
                    ShortDescription = model.ShortDescription,
                    FeaturedImgUrl = model.FeaturedImgUrl,
                    UrlHandle = model.UrlHandle,
                    PublishedDate = model.PublishedDate,
                    Author = model.Author,
                    isVisible = model.isVisible,
                    // Convert the Tags property to a list (so the user can select)
                    Tags = tagsDomainModel
                    .Select(q => new SelectListItem
                    {
                        Text = q.DisplayName,
                        Value = q.Id.ToString()
                    }),
                    // Display user selected tags
                    SelectedTags = model.Tags.Select(q => q.Id.ToString()).ToArray()
                };
                return View(viewModel);
            }
            // return View(viewModel);
            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Update(EditBlogPostRequestVM viewModel)
        {
            // Map View Model back to Domain Model
            var domainModel = new BlogPost
            {
                Id = viewModel.Id,
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

            // Map Tags to Domain Mo
            // Loop, and check if a tag do exist for a record, then model-bind it to model var
            var selectedTags = new List<Tag>();
            foreach (var selectedTag in viewModel.SelectedTags)
            {
                // if Id exists (but before that parse it to string)
                if (Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await _tagRepository.Get(tag);
                    if (foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }
                domainModel.Tags = selectedTags;
            }
            // Submit to repository and update
            var updatedBlog = await _blogPostRepository.Update(domainModel);

            if (updatedBlog != null)
            {
                // Redirect to Index
                return RedirectToAction(nameof(Index));
            }
            // Error finding or saving record
            return RedirectToAction(nameof(Add));
        }

        // Delete
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var model = await _blogPostRepository.Get(id);
            // Map Domain Model to View Model
            if (model != null)
            {
                var viewModel = new ReadOnlyBlogPostRequestVM
                {
                    Id = model.Id,
                    Heading = model.Heading,
                };
                return View(viewModel);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(Guid id)
        {
            await _blogPostRepository.Delete(id);
            return RedirectToAction(nameof(Index));

            //    return RedirectToAction(nameof(Add));
        }

        #endregion
    }
}

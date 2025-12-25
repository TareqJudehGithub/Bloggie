using Bloggie.Models.Domain;
using Bloggie.Models.ViewModel;
using Bloggie.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostLikeController : ControllerBase
    {
        #region Fields
        private readonly IBlogPostLikesRepository _blogPostLikesRepository;
        #endregion

        #region Constructors
        public BlogPostLikeController(IBlogPostLikesRepository blogPostLikesRepository)
        {
            _blogPostLikesRepository = blogPostLikesRepository;
        }
        #endregion
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddLike([FromBody] AddLikeRequestVM addLikeRequestVM)
        {
            var model = new BlogPostLike
            {
                BlogPostId = addLikeRequestVM.BlogPostId,
                UserId = addLikeRequestVM.UserId
            };

            await _blogPostLikesRepository.AddLikeForBlogPost(blogPostLike: model);

            return Ok();
        }

        // Get total number of likes for a blog post
        [HttpGet]
        [Route("{blogPostId:Guid}/totalLikes")]
        public async Task<IActionResult> GetTotalLikesForBlog([FromRoute] Guid blogPostId)
        {
            var totalLikes = await _blogPostLikesRepository.GetTotalLikes(blogPostId: blogPostId);
            return Ok(totalLikes);
        }
    }
}




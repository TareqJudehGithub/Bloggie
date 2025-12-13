using Bloggie.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Bloggie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {

        #region Fields
        private readonly IImageRepository _imageRepository;
        #endregion

        #region Constructors
        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }
        #endregion

        #region Action Methods
        public async Task<IActionResult> UploadAsync(IFormFile file)
        // This method lets us upload an image into the cloud, using Fraola
        {
            // call a repository
            var imageUrl = await _imageRepository.UploadAsync(file);

            // If image upload did not work
            if (imageUrl == null)
            {
                return Problem(
                    "Something went wrong!",
                    null,
                    (int)HttpStatusCode.InternalServerError);
            }
            return new JsonResult(new { link = imageUrl });
        }
        #endregion
    }
}

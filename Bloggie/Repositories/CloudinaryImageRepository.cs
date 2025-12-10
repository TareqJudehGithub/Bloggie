using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Bloggie.Repositories
{
    public class CloudinaryImageRepository : IImageRepository
    {
        #region Fields
        private readonly IConfiguration _configuration;
        // Account class is from CloudinaryDotNet
        private readonly Account _account;
        #endregion

        #region Constructor
        public CloudinaryImageRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _account = new Account(
                _configuration.GetSection("Cloudinary")["CloudName"],
                _configuration.GetSection("CLoudinary")["ApiKey"],
                _configuration.GetSection("Cloudinary")["ApiSecret"]
                );
        }
        #endregion

        #region Methods
        public async Task<string> UploadAsync(IFormFile file)
        {
            // Create a new account
            var client = new Cloudinary(_account);

            // Upload an image 
            //=================
            var uploadParams = new ImageUploadParams()   // from CloudinaryDotNet.Actions;
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                DisplayName = file.FileName
            };
            var uploadResult = await client.UploadAsync(uploadParams);

            if (uploadResult != null &&
                uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUrl.ToString();
            }
            return null;
        }
        #endregion
    }
}


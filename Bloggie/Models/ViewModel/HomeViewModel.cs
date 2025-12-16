using Bloggie.Models.Domain;

namespace Bloggie.Models.ViewModel
{
    public class HomeViewModel
    {
        public IEnumerable<ReadOnlyBlogPostRequestVM> BlogPosts { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }
}

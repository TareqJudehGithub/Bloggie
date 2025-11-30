using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggie.Models.ViewModel
{
    public class AddBlogPostRequestVM
    {
        public string Heading { get; set; }
        public string PageTitle { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public string FeaturedImgUrl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public bool isVisible { get; set; }

        // Display tags
        // SelectLIstItem class is from Microsoft.AspNetCore.Mvc.Rendering;
        public IEnumerable<SelectListItem> Tags { get; set; }

        // Collect/capture a tag
        // Single Tag
        public string SelectedTag { get; set; }

        // Multiple tags
        public string[] SelectedTags { get; set; } = Array.Empty<string>();
    }
}

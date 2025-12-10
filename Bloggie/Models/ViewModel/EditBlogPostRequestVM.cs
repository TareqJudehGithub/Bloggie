using Bloggie.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Bloggie.Models.ViewModel
{
    public class EditBlogPostRequestVM
    {
        public Guid Id { get; set; }
        public string Heading { get; set; }
        public string PageTitle { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public string FeaturedImgUrl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public bool isVisible { get; set; }

        // Navigation property - which tells EF Core that this is a related property.
        public IEnumerable<SelectListItem> Tags { get; set; }

        // Collect/capture a tag
        // Single Tag
        public string SelectedTag { get; set; }

        // Multiple tags
        public string[] SelectedTags { get; set; } = Array.Empty<string>();
    }
}

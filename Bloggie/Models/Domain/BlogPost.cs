using System.ComponentModel.DataAnnotations.Schema;

namespace Bloggie.Models.Domain
{
    [Table(name: "BlogPosts", Schema = "dbo")]
    public class BlogPost
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
        public ICollection<Tag> Tags { get; set; }
    }
}

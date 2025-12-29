using System.ComponentModel.DataAnnotations.Schema;

namespace Bloggie.Models.Domain
{
    [Table(name: "Comments", Schema = "dbo")]
    public class BlogPostComment
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
        public Guid BlogPostId { get; set; }
        public DateTime DateAdded { get; set; }
    }
}

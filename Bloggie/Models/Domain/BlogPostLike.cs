using System.ComponentModel.DataAnnotations.Schema;

namespace Bloggie.Models.Domain
{
    [Table(name: "Likes", Schema = "dbo")]
    public class BlogPostLike
    {
        public Guid Id { get; set; }
        public Guid BlogPostId { get; set; }
        public Guid UserId { get; set; }
    }
}

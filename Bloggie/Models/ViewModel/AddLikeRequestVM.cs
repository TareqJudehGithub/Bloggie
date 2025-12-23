namespace Bloggie.Models.ViewModel
{
    public class AddLikeRequestVM
    {
        public Guid BlogPostId { get; set; }
        public Guid UserId { get; set; }
    }
}

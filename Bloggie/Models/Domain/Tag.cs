using System.ComponentModel.DataAnnotations.Schema;

namespace Bloggie.Models.Domain
{
    [Table(name: "Tags", Schema = "dbo")]
    public class Tag
    {
        #region Properties
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        #endregion

        public ICollection<BlogPost> BlogPosts { get; set; }
    }
}

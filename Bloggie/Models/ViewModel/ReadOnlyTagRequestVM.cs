using System.ComponentModel.DataAnnotations;

namespace Bloggie.Models.ViewModel
{
    public class ReadOnlyTagRequestVM
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Tag Code")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Tag Description")]
        public string DisplayName { get; set; }
    }
}



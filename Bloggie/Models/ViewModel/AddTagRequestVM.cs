using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bloggie.Models.ViewModel
{
    public class AddTagRequestVM
    {
        [Required]
        [Display(Name = "Tag Code")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Tag Description")]
        public string DisplayName { get; set; }
    }
}

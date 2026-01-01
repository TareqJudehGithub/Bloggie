using System.ComponentModel.DataAnnotations;

namespace Bloggie.Models.ViewModel
{
    public class RegisterVM
    {
        #region Properties
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password should be at least {1} characters")]
        public string Password { get; set; }
        #endregion
    }
}

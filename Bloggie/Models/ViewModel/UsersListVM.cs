namespace Bloggie.Models.ViewModel
{
    public class UsersListVM
    {
        public List<UserVM> Users { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsUser { get; set; }
        public bool IsAdmin { get; set; }

    }
}

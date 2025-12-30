using Microsoft.AspNetCore.Mvc;

using Bloggie.Models.ViewModel;
using Bloggie.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


namespace Bloggie.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : Controller
    {
        #region Fields
        private readonly IUserRepository _userRepository;
        #endregion
        #region Constructors
        public AdminUsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        #endregion
        #region Action Methods
        public async Task<IActionResult> Index()

        // This method displays all users
        {
            // Get all users 
            var model = await _userRepository.GetAllUsersAsync();
            var viewModel = new UsersListVM();

            if (ModelState.IsValid && model != null)
            {
                viewModel.Users = new List<UserVM>();

                // Iterate over every user in model, creating a new list of Users in the view model.
                foreach (var user in model)
                {
                    viewModel.Users.Add(new UserVM
                    {
                        Id = Guid.Parse(user.Id),
                        Username = user.UserName,
                        Email = user.Email
                    });
                }
                if (viewModel.Users.Count == 0)
                {
                    TempData["AlertType"] = "info";
                    TempData["AlertMessage"] = "Users list is empty.";
                }
                return View(viewModel);
            }
            TempData["AlertType"] = "danger";
            TempData["AlertMessage"] = "Error loading Users list.";
            return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AddUserRequestVM viewModel)
        {
            // Convert from view to domain model
            var model = new IdentityUser
            {
                Id = viewModel.Id.ToString(),
                UserName = viewModel.Username,
                PasswordHash = viewModel.Password,
                Email = viewModel.Email
            };
            await _userRepository.AddAsync(model);

            return RedirectToAction(actionName: nameof(Index), controllerName: "AdminUsers");
        }
        #endregion
    }
}

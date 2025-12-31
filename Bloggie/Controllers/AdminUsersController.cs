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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserRepository _userRepository;
        #endregion
        #region Constructors
        public AdminUsersController(
            UserManager<IdentityUser> userManager,
            IUserRepository userRepository
            )
        {
            _userManager = userManager;
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
        public async Task<IActionResult> Index(UsersListVM viewModel)
        {
            var identityUser = new IdentityUser
            {
                UserName = viewModel.Username,
                Email = viewModel.Email
            };
            // Create user in the database
            var identityResult = await _userManager.CreateAsync(user: identityUser, password: viewModel.Password);

            if (identityResult is not null)
            {
                if (identityResult.Succeeded)
                {
                    // Assign role "User" to the new user
                    List<string> roles = new List<string>();

                    if (viewModel.IsUser)
                    {
                        roles.Add("User");
                    }
                    //var roles = new List<string> { "User" };

                    //  Check if user is admin
                    if (viewModel.IsAdmin)
                    {
                        roles.Add("Admin");
                    }
                    // Assign roles create (the roles list) to the new user
                    identityResult = await _userManager.AddToRolesAsync(user: identityUser, roles: roles);
                    TempData["AlertType"] = "success";
                    TempData["AlertMessage"] = $"User {viewModel.Username} was successfully created.";
                    return RedirectToAction(controllerName: "AdminUsers", actionName: nameof(Index));
                }
            }


            TempData["AlertType"] = "danger";
            TempData["AlertMessage"] = "Error creating new user.";
            return View();
        }


        // For the Add view - alt creating new users
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

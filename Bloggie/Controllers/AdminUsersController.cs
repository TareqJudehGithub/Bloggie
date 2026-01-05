using Bloggie.Models.ViewModel;
using Bloggie.Repositories;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;


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
        public async Task<IActionResult> Index(
            string? searchQuery,
            string? sortBy,
            string? sortDirection
            )
        // This method displays all users
        {
            // Save search state after submission a search request
            ViewBag.SearchQuery = searchQuery;

            // Sorting 
            ViewBag.SortBy = sortBy;
            ViewBag.SortDirection = sortDirection;

            // Get all users 
            var model = await _userRepository.GetAllUsersAsync(searchQuery, sortBy, sortDirection);
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

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {

            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user is not null)
            {

                // Get roles for this user
                var roles = await _userManager.GetRolesAsync(user);

                // Check if the logged user is a manger, and prevent him/her from deleting other users
                // with Manager roles.
                if (!User.IsInRole("SuperAdmin") && User.IsInRole("Admin"))
                {
                    if (roles.Contains("Admin"))
                    {
                        TempData["AlertType"] = "danger";
                        TempData["AlertMessage"] = "Only a Super Admin can delete users with a Manager role.";

                        ModelState.AddModelError(key: "", errorMessage: "Cannot delete a user with a Manager role.");
                        return RedirectToAction(controllerName: "AdminUsers", actionName: nameof(Index));
                    }
                }
                // Delete the found user
                await _userManager.DeleteAsync(user);
                TempData["AlertType"] = "success";
                TempData["AlertMessage"] = $"{user.UserName} was successfully deleted.";
                return RedirectToAction(controllerName: "AdminUsers", actionName: nameof(Index));
            }
            // In case the user was not found, display a message and redirect back to the users list.
            TempData["AlertType"] = "danger";
            TempData["AlertMessage"] = $"{user.UserName} was not found.";
            return RedirectToAction(controllerName: "AdminUsers", actionName: nameof(Index));
        }
        #endregion
    }
}

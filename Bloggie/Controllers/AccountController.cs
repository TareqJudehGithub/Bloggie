using Bloggie.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace Bloggie.Controllers
{
    public class AccountController : Controller
    {
        #region Fields
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        #endregion

        #region Constructor
        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        #endregion

        #region Methods
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            // New user object
            var identityUser = new IdentityUser
            {
                UserName = registerVM.Username,
                Email = registerVM.Email

            };
            // Create a user using UserManager with a password
            var identityResult = await _userManager.CreateAsync(user: identityUser, password: registerVM.Password);

            if (identityResult.Succeeded)
            {
                // Assign this user a user role, and store the result in the database
                var roleIdentityResult = await _userManager.AddToRoleAsync(user: identityUser, role: "User");
                if (roleIdentityResult.Succeeded)
                {
                    return RedirectToAction(nameof(Register));
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            var model = new LoginVM
            {
                ReturnUrl = ReturnUrl
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(
                userName: loginVM.Username,
                password: loginVM.Password,
                isPersistent: false,
                lockoutOnFailure: false
                );

            if (signInResult != null && signInResult.Succeeded)
            {

                if (!string.IsNullOrWhiteSpace(loginVM.ReturnUrl))
                {
                    return Redirect(loginVM.ReturnUrl);
                }
                return RedirectToAction(controllerName: "Home", actionName: "Index");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Logout(LoginVM loginVM)
        {

            await _signInManager.SignOutAsync();

            return RedirectToAction(controllerName: "Account", actionName: nameof(Login));
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            TempData["AlertType"] = "danger";
            TempData["AlertMessage"] = "Access Denied! You need Admin privileges in order to access this page!";

            return View();
        }
        #endregion
    }
}


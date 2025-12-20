using Bloggie.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace Bloggie.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        public AccountController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            // User 
            var identityUser = new IdentityUser
            {
                UserName = registerVM.Username,
                Email = registerVM.Email,
            };

            var identityResult = await _userManager.CreateAsync(user: identityUser, password: registerVM.Password);

            if (identityResult.Succeeded)
            {
                // Assign this user, a User role
                var roleIdentityResult = await _userManager.AddToRoleAsync(user: identityUser, role: "User");
                if (roleIdentityResult.Succeeded)
                {
                    return RedirectToAction(nameof(Register));
                }

            }

            return View();
        }
    }
}

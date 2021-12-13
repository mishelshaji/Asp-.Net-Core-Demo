using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromForm]LoginViewModel model)
        {
            if(!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "No matching accounts found");
                return View(model);
            }

            var signinStatus = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, isPersistent: true, lockoutOnFailure: true);
            if (signinStatus.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (signinStatus.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Your account is blocked. Please try after some time");
                return View(model);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login details");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm]RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if(await _userManager.FindByEmailAsync(model.Email) != default)
            {
                ModelState.AddModelError(nameof(model.Email), "This email is used by another user");
            }

            if (!ModelState.IsValid)
                return View(model);

            var user = new ApplicationUser()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = Guid.NewGuid().ToString().Replace("-", string.Empty),
            };

            var userCreatedStatus = await _userManager.CreateAsync(user, model.Password);
            if (userCreatedStatus.Succeeded)
            {
                return RedirectToAction(nameof(Login));
            }

            foreach(var error in userCreatedStatus.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return LocalRedirect("/");
        }
    }
}

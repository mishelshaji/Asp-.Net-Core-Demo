using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountController(
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Login()
        {
            return View();
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
    }
}

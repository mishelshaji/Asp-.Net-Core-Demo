using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AspStore.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _db.Carts.Include(m=>m.Product).Where(m => m.ApplicationUserId == userId)
                .ToListAsync();

            var totalAmount = 0;
            foreach (var cartItem in cart)
            {
                totalAmount += cartItem.Product.SalesPrice * cartItem.Quantity;
            }
            ViewBag.TotalAmount = totalAmount;
            return View(cart);
        }

        [HttpGet]
        public async Task<IActionResult> Add(long id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            var cartEntry = await _db.Carts.Where(m=>m.ProductId == id && m.ApplicationUserId == user.Id).
                FirstOrDefaultAsync();
            if (cartEntry == null)
            {
                await _db.Carts.AddAsync(new Cart()
                {
                    ProductId = id,
                    ApplicationUserId = user.Id,
                    CreatedOn = DateTime.UtcNow,
                });
            }
            else
            {
                cartEntry.Quantity += 1;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Delete(long id)
        {
            var cart = await _db.Carts.FindAsync(id);
            if (cart == default)
            {
                return NotFound();
            }
            _db.Remove(cart);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Route("[controller]/[action]/{id}/{task}")]
        public async Task<IActionResult> UpdateQuantity(long id, string task)
        {
            var cartItem = await _db.Carts.FindAsync(id);
            if (cartItem == null)
                return NotFound();

            if (task == "add")
                cartItem.Quantity++;
            else if(task == "sub")
                cartItem.Quantity--;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(this.Index));
        }

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AspStore.Areas.Admin.Controllers
{
    [Authorize(Roles = $"{UserRoles.Admin}")]
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductController(
            ApplicationDbContext db,
            IWebHostEnvironment webHostEnvironment,
            UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _db.Products.Include(m=>m.Brand)
                .Include(m=>m.Category).ToListAsync();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View(new ProductViewModel()
            {
                Brands = await _db.Brands.ToListAsync(),
                Categories = await _db.Categories.ToListAsync()
            });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create([FromForm]ProductViewModel model)
        {
            ModelState.Remove(nameof(model.Categories));
            ModelState.Remove(nameof(model.Brands));
            model.Brands = await _db.Brands.ToListAsync();
            model.Categories = await _db.Categories.ToListAsync();

            if (!ModelState.IsValid)
                return View(model);

            var exists = await _db.Products.AnyAsync(m=>m.Name == model.Name ||
                m.Slug == model.Slug);

            if (exists)
            {
                ModelState.AddModelError(string.Empty, "A similar product already exists");
                return View(model);
            }

            if (model.SalesPrice > model.RegularPrice)
            {
                ModelState.AddModelError(nameof(model.SalesPrice), "Should not be greater than regular price");
                return View(model);
            }

            var uploadPath = Path.Combine("uploads", "product", "images");
            var absolutePath = Path.Combine(_webHostEnvironment.WebRootPath, uploadPath);
            var savedFileName = await model.Image.SaveFileToDiskAsync(absolutePath);

            var userFromDb = await _userManager.GetUserAsync(User);
            await _db.Products.AddAsync(new Product()
            {
                BrandId = model.BrandId,
                CategoryId = model.CategoryId,
                MetaDescription = model.MetaDescription,
                Description = model.Description,
                Image = Path.Combine(uploadPath, savedFileName).Replace("\\", "/"),
                Name = model.Name,
                Slug = model.Slug,
                SalesPrice = model.SalesPrice,
                RegularPrice = model.RegularPrice,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
                ApplicationUserId = userFromDb.Id
                //ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            });
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == default)
                return NotFound();

            var product = await _db.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            return View(new ProductEditViewModel()
            {
                BrandId = product.BrandId,
                CategoryId = product.CategoryId,
                Name = product.Name,
                Slug = product.Slug,
                Description = product.Description,
                MetaDescription = product.MetaDescription,
                RegularPrice = product.RegularPrice,
                SalesPrice = product.SalesPrice,
                Categories = await _db.Categories.ToListAsync(),
                Brands = await _db.Brands.ToListAsync(),
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute]int id, [FromForm]ProductEditViewModel model)
        {
            model.Brands = await _db.Brands.ToListAsync();
            model.Categories = await _db.Categories.ToListAsync();

            if (id == default)
                return NotFound();

            var product = await _db.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            var uploadPath = Path.Combine("uploads", "product", "images");
            var absolutePath = Path.Combine(_webHostEnvironment.WebRootPath, uploadPath);
            var savedFileName = string.Empty;
            if (model.Image != default)
            {
                var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, product.Image);
                savedFileName = await model.Image.SaveFileToDiskAsync(absolutePath);
                System.IO.File.Delete(oldFilePath);
                product.Image = Path.Combine(uploadPath, savedFileName);
            }

            product.UpdatedOn = DateTime.UtcNow;
            await TryUpdateModelAsync(product, string.Empty,
                m => m.Name,
                m => m.Description,
                m => m.MetaDescription,
                m => m.RegularPrice,
                m => m.SalesPrice,
                m => m.Slug);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == default)
                return NotFound();

            var product = await _db.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName(nameof(Delete))]
        public async Task<IActionResult> DeleteConfirmation(int id)
        {
            if (id == default)
                return NotFound();

            var product = await _db.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            _db.Remove(product);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(this.Index));
        }
    }
}

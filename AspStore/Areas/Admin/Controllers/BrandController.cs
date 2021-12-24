using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspStore.Areas.Admin.Controllers
{
    [Authorize(Roles = $"{UserRoles.Admin}")]
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BrandController(
            ApplicationDbContext db,
            IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _db.Brands.ToListAsync();
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create([FromForm]BrandViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var exists = await _db.Brands.AnyAsync(m=>m.Name == model.Name ||
                m.Slug == model.Slug || m.SupportEmail == model.SupportEmail);
            if (exists)
            {
                ModelState.AddModelError(string.Empty, "A similar brand already exists");
                return View(model);
            }

            var uploadPath = Path.Combine("uploads", "brand", "logos");
            var absolutePath = Path.Combine(_webHostEnvironment.WebRootPath, uploadPath);
            var savedFileName = await model.Logo.SaveFileToDiskAsync(absolutePath);

            await _db.Brands.AddAsync(new Brand()
            {
                Description = model.Description,
                Logo = Path.Combine(uploadPath, savedFileName).Replace("\\", "/"),
                Name = model.Name,
                OffictalWebsite = model.OffictalWebsite,
                Slug = model.Slug,
                SupportEmail = model.SupportEmail,
            });
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == default)
                return NotFound();

            var brand = await _db.Brands.FindAsync(id);
            if (brand == null)
                return NotFound();

            var model = new BrandEditViewModel()
            {
                Description = brand.Description,
                ExistingLogo = brand.Logo.GetAsFileUploadPath(),
                Name = brand.Name,
                OffictalWebsite = brand.OffictalWebsite,
                Slug = brand.Slug,
                SupportEmail = brand.SupportEmail,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute]int id, [FromForm]BrandEditViewModel model)
        {
            if (id == default)
                return NotFound();

            var brand = await _db.Brands.FindAsync(id);
            if (brand == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            var uploadPath = Path.Combine("uploads", "brand", "logos");
            var absolutePath = Path.Combine(_webHostEnvironment.WebRootPath, uploadPath);
            var savedFileName = string.Empty;
            if (model.Logo != default)
            {
                var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, brand.Logo);
                savedFileName = await model.Logo.SaveFileToDiskAsync(absolutePath);
                System.IO.File.Delete(oldFilePath);
                brand.Logo = Path.Combine(uploadPath, savedFileName);
            }

            await TryUpdateModelAsync(brand, string.Empty,
                m => m.Name,
                m => m.Description,
                m => m.Slug,
                m => m.OffictalWebsite,
                m => m.SupportEmail);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == default)
                return NotFound();

            var brand = await _db.Brands.FindAsync(id);
            if (brand == null)
                return NotFound();

            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName(nameof(Delete))]
        public async Task<IActionResult> DeleteConfirmation(int id)
        {
            if (id == default)
                return NotFound();

            var brand = await _db.Brands.FindAsync(id);
            if (brand == null)
                return NotFound();

            _db.Remove(brand);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(this.Index));
        }
    }
}

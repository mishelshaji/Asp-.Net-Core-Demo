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

        public IActionResult Index()
        {
            return View();
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
            var uniqueFileName = Guid.NewGuid().ToString();
            var fileExtension = Path.GetExtension(model.Logo.FileName);
            if(!(fileExtension.Equals(".jpg") || fileExtension.Equals(".png")))
            {
                ModelState.AddModelError(nameof(model.Logo), "Invalid file type");
                return View(model);
            }

            var uploadTo = Path.Combine(absolutePath, uniqueFileName + fileExtension);
            try
            {
                Directory.CreateDirectory(absolutePath);
                using (var stream = new FileStream(uploadTo, FileMode.Create))
                {
                    await model.Logo.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            await _db.Brands.AddAsync(new Brand()
            {
                Description = model.Description,
                Logo = Path.Combine(uploadPath, uniqueFileName + fileExtension).Replace("\\", "/"),
                Name = model.Name,
                OffictalWebsite = model.OffictalWebsite,
                Slug = model.Slug,
                SupportEmail = model.SupportEmail,
            });
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(this.Index));
        }
    }
}

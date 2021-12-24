using AspStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AspStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext db
           )
        {
            _logger = logger;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _db.Products.Include(m=>m.Category).Include(m=>m.Brand)
                .OrderBy(m=>m.CreatedOn).ToListAsync();
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Test()
        {
            ViewBag.Message = "@Datetime.Now";
            return View();
        }
    }
}
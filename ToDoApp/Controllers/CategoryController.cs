using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Data;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _db.Categories.ToListAsync();
            return View(data);
        }

        [HttpGet]
        //[Route("[controller]/new")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _db.AddAsync(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        //[Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            if(id == default)
                return NotFound();

            var categoryFromDb = await _db.Categories.FindAsync(id);

            if (categoryFromDb == null)
                return NotFound();

            return View(categoryFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> Edit([FromRoute]int id, [FromForm]Category model)
        {
            if (id == default)
                return NotFound();

            if(!ModelState.IsValid)
                return View(model);

            var categoryFromDb = await _db.Categories.FindAsync(id);

            if (categoryFromDb == null)
                return NotFound();

            categoryFromDb.Name = model.Name;
            categoryFromDb.Description = model.Description;

            //_db.Update(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            if (id == default)
                return NotFound();

            var categoryFromDb = await _db.Categories.FindAsync(id);

            if (categoryFromDb == null)
                return NotFound();

            return View(categoryFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, [FromForm]Category model)
        {
            if (id == default)
                return NotFound();

            var categoryFromDb = await _db.Categories.FindAsync(id);

            if (categoryFromDb == null)
                return NotFound();

            _db.Remove(categoryFromDb);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

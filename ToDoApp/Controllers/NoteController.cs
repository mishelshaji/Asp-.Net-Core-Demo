using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Data;
using ToDoApp.Models.ViewModels;

namespace ToDoApp.Controllers
{
    public class NoteController : Controller
    {
        private readonly ApplicationDbContext _db;
        public NoteController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: NoteController
        public async Task<ActionResult> Index()
        {
            return View();
        }

        // GET: NoteController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: NoteController/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new NoteViewModel()
            {
                Categories = await _db.Categories.OrderBy(m => m.Name).ToListAsync()
            };
            return View(model);

            // Short syntax
            //return View(new NoteViewModel()
            //{
            //    Categories = await _db.Categories.OrderBy(m => m.Name).ToListAsync()
            //});
        }

        // POST: NoteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] NoteViewModel model)
        {
            var categories = await _db.Categories.ToListAsync();
            model.Categories = categories;
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _db.Notes.AddAsync(new Models.Note()
            {
                Body = model.Body,
                CategoryId = model.CategoryId,
                ExpiresOn = model.ExpiresOn,
                Title = model.Title,
            });
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: NoteController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: NoteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: NoteController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: NoteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

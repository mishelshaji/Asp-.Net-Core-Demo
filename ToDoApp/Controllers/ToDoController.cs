using Microsoft.AspNetCore.Mvc;
using ToDoApp.Data;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ToDoController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var data = _db.ToDos.OrderBy(m=>m.CompleteOn).ToList();
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
        public IActionResult Create(ToDo model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _db.Add(model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        //[Route("[controller]/[action]/{id}")]
        public IActionResult Edit(int id)
        {
            if(id == default)
                return NotFound();

            var todoFromDb = _db.ToDos.Find(id);

            if (todoFromDb == null)
                return NotFound();

            return View(todoFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Route("[controller]/[action]/{id}")]
        public IActionResult Edit([FromRoute]int id, [FromForm]ToDo model)
        {
            if (id == default)
                return NotFound();

            if(!ModelState.IsValid)
                return View(model);

            var todoFromDb = _db.ToDos.Find(id);

            if (todoFromDb == null)
                return NotFound();

            todoFromDb.Name = model.Name;
            todoFromDb.Description = model.Description;
            todoFromDb.CompleteOn = model.CompleteOn;

            //_db.Update(model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete([FromRoute]int id)
        {
            if (id == default)
                return NotFound();

            var todoFromDb = _db.ToDos.Find(id);

            if (todoFromDb == null)
                return NotFound();

            return View(todoFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, [FromForm]ToDo model)
        {
            if (id == default)
                return NotFound();

            var todoFromDb = _db.ToDos.Find(id);

            if (todoFromDb == null)
                return NotFound();

            _db.Remove(todoFromDb);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult UpdateStatus([FromRoute]int id)
        {
            if (id == default)
                return NotFound();

            var todoFromDb = _db.ToDos.Find(id);

            if (todoFromDb == null)
                return NotFound();

            todoFromDb.HasCompleted = !todoFromDb.HasCompleted;
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}

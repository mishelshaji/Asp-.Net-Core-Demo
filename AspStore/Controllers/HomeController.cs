using AspStore.Dependencies;
using AspStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AspStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMessageService _test;

        public HomeController(ILogger<HomeController> logger, 
            IMessageService test,
            ScopedTester s)
        {
            _logger = logger;
            _test = test;
        }

        public IActionResult Index()
        {
            return View();
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
            return Content(_test.GetGreetingMessage());
        }
    }
}
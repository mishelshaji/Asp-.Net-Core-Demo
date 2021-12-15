using Microsoft.AspNetCore.Mvc;

namespace AspStore.Controllers
{
    public class AppInitController : Controller
    {
        public AppInitController(SyncAppData appData)
        {

        }

        public IActionResult Index()
        {
            return Ok("App running");
        }
    }
}

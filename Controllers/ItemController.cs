using Microsoft.AspNetCore.Mvc;

namespace WebInventoryManagement.Controllers
{
    public class ItemController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public ItemController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}

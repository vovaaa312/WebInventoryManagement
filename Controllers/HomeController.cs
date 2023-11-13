using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebInventoryManagement.Models;

namespace WebInventoryManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("/Home/Error")]
        public IActionResult Error()
        {
            var statusCode = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            if (statusCode != null)
            {
                string statusCodeValue = statusCode.OriginalPath + statusCode.OriginalQueryString + " resulted in status code " + statusCode.OriginalPathBase;

                if (int.TryParse(statusCodeValue, out int statusCodeInt))
                {
                    return View(new ErrorViewModel { StatusCode = statusCodeInt });
                }
            }

            return View(new ErrorViewModel { StatusCode = 0 });
        }

    }
}
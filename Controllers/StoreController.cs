using Microsoft.AspNetCore.Mvc;
using WebInventoryManagement.Models;
using WebInventoryManagement.Models.Data;
using WebInventoryManagement.Services;

namespace WebInventoryManagement.Controllers
{
    public class StoreController : Controller
    {
        StoreService storeService;

        public StoreController(StoreService _storeService)
        {
            this.storeService = _storeService;
        }

        private readonly ILogger<HomeController> _logger;



        public ActionResult Index()
        {
            IEnumerable<Store> stores = storeService.GetAll();
            return View(stores);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Store store)
        {
            if (ModelState.IsValid)
            {
                storeService.AddStore(store);
                return RedirectToAction("Index"); // Перенаправить на экшен Index
            }
            return View(); // В случае ошибки возвращаем ту же форму для исправления
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            
                // Найдите элемент по id
                var store = storeService.GetById(id);

                if (store == null)
                {
                    return NotFound();
                }

                // Удалите элемент
                storeService.DeleteStore(store);

                return RedirectToAction("Index"); // Перенаправить на экшен Index           
            
        }

        public IActionResult Edit(int id)
        {
            var store = storeService.GetById(id);
            if (store == null)
            {
                return NotFound(); // Handle the case where the store is not found.
            }
            return View("Edit", store);
        }

        [HttpPost]
        public IActionResult Update(Store store)
        {
            if (ModelState.IsValid)
            {
                storeService.UpdateStore(store);
                return RedirectToAction("Index");
            }
            return View("Edit", store);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("/Shared/Error")]
        public ActionResult Error(int? statusCode)
        {
            var errorModel = new ErrorViewModel();

            if (statusCode.HasValue)
            {
                errorModel.StatusCode = statusCode.Value;
                errorModel.ErrorMessage = "An error occurred with status code: " + statusCode.Value;
            }
            else
            {
                errorModel.StatusCode = 500; // Internal Server Error
                errorModel.ErrorMessage = "An unexpected error occurred.";
            }

            return View(errorModel);
        }
        






    }
}

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
                storeService.Save(store);
                return RedirectToAction("Index"); // Перенаправить на экшен Index
            }
            return View(); // В случае ошибки возвращаем ту же форму для исправления
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                var store = storeService.GetById(id);

                if (store == null)
                {
                    return NotFound();
                }

                storeService.Delete(store);
                return RedirectToAction("Index");
            }
            catch (Exception ex) 
            {
                if (ex.Message.Contains("ORA-02292"))
                {
                    return RedirectToAction("Error", new { message = "The Store cannot be deleted because there are Shelf entries associated with it." });
                }
                throw;
            }
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
            if (ModelState.IsValid && store.StoreName != "" && store.StoreName!=null)
            {
                storeService.Update(store);
                return RedirectToAction("Index");
            }
            return View("Edit", store);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("[controller]/Error")]
        public ActionResult Error(int? statusCode, string message = null)
        {
            return ErrorHandler.Error(this, statusCode, message);
        }







    }
}

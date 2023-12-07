using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebInventoryManagement.Models.Data;
using WebInventoryManagement.Services;

namespace WebInventoryManagement.Controllers
{
    public class ShelfController : Controller
    {
        private ShelfService shelfService;

        private StoreService storeService;

        private CategoryService categoryService;

        private readonly ILogger<HomeController> _logger;

        //public ShelfController(ShelfService _shelfService)
        //{
        //    this.shelfService = _shelfService;
        //}

        public ShelfController(ShelfService shelfService, StoreService storeService, CategoryService categoryService)
        {
            this.shelfService = shelfService;
            this.storeService = storeService;
            this.categoryService = categoryService;
        }

        public ActionResult Index()
        {
            IEnumerable<Shelf> shelves = shelfService.GetAll();
            LoadViewBagStoresList();

            return View(shelves);
        }

        [HttpGet]
        public ActionResult Create()
        {
            // Получите список категорий из вашего сервиса или базы данных
            LoadViewBagCategoriesList();
            LoadViewBagStoresList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Shelf shelf)
        {

            if (shelf.ShelfName != null)
            {
                shelf.Store = (from store in storeService.GetAll() where store.Id == shelf.StoreId select store).First();
                shelf.Category = (from category in categoryService.GetAll() where category.Id == shelf.CategoryId select category).First();

                shelfService.Save(shelf);
                return RedirectToAction("Index");// Перенаправить на экшен Index
            }

            LoadViewBagCategoriesList();
            LoadViewBagStoresList();

            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {


            try
            {
                var shelf = shelfService.GetById(id);

                if (shelf == null)
                {
                    return NotFound();
                }

                shelfService.Delete(shelf);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("ORA-02292"))
                {

                    return RedirectToAction("Error", new { message = "The Shelf cannot be deleted because there are Item entries associated with it." });
                }
                throw;
            }

        }


        public IActionResult Edit(int id)
        {
            var shelf = shelfService.GetById(id);
            if (shelf == null)
            {
                return NotFound();
            }
            LoadViewBagCategoriesList();
            LoadViewBagStoresList();

            return View("Edit", shelf);
        }
        [HttpPost]
        public IActionResult Update(Shelf shelf)
        {
            if (shelf.ShelfName != null)
            {
                shelf.Store = (from store in storeService.GetAll() where store.Id == shelf.StoreId select store).First();
                shelf.Category = (from category in categoryService.GetAll() where category.Id == shelf.CategoryId select category).First();


                shelfService.Update(shelf);
                return RedirectToAction("Index");
            }
            return View("Edit", shelf);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("[controller]/Error")]
        public ActionResult Error(int? statusCode, string message = null)
        {
            return ErrorHandler.Error(this, statusCode, message);
        }



        private void LoadViewBagStoresList()
        {
            ViewBag.StoreList = ViewBagLoader.LoadViewBagList(storeService.GetAll(), "Id", "StoreName");
        }
        private void LoadViewBagCategoriesList()
        {
            ViewBag.CategoryList = ViewBagLoader.LoadViewBagList(categoryService.GetAll(), "Id", "CategoryName");
        }


    }
}

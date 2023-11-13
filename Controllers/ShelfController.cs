using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebInventoryManagement.Models.Data;
using WebInventoryManagement.Services;

namespace WebInventoryManagement.Controllers
{
    public class ShelfController : Controller
    {
        private ShelfService shelfService;

        public ShelfController(ShelfService _shelfService)
        {
            this.shelfService = _shelfService;
        }

        private readonly ILogger<HomeController> _logger;



        public ActionResult Index()
        {
            IEnumerable<Shelf> shelves = shelfService.GetAllShelves();
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
                shelf.Store = (from store in shelfService.GetAllStores() where store.Id == shelf.StoreId select store).First();
                shelf.Category = (from category in shelfService.GetAllCategories() where category.Id == shelf.CategoryId select category).First();

                shelfService.AddShelf(shelf);
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
            var shelf = shelfService.GetById(id);

            if (shelf == null)
            {
                return NotFound();
            }

            shelfService.DeleteShelf(shelf);

            return RedirectToAction("Index");

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
                shelf.Store = (from store in shelfService.GetAllStores() where store.Id == shelf.StoreId select store).First();
                shelf.Category = (from category in shelfService.GetAllCategories() where category.Id == shelf.CategoryId select category).First();


                shelfService.UpdateShelf(shelf);
                return RedirectToAction("Index");
            }
            return View("Edit", shelf);
        }
       


        private void LoadViewBagStoresList() {
            ViewBag.StoreList = new SelectList(shelfService.GetAllStores(), "Id", "StoreName"); 
        }
        private void LoadViewBagCategoriesList() {
            ViewBag.CategoryList = new SelectList(shelfService.GetAllCategories(), "Id", "CategoryName");
        }


    }
}

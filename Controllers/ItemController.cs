using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.Design;
using WebInventoryManagement.Models.Data;
using WebInventoryManagement.Services;

namespace WebInventoryManagement.Controllers
{
    public class ItemController : Controller
    {
        private readonly ILogger<HomeController>? _logger;

        private ItemService itemService;

        private ShelfService shelfService;

        private CategoryService categoryService;

        private StoreService storeService;



        public ItemController(ItemService itemService, ShelfService shelfService, CategoryService categoryService, StoreService storeService)
        {
            this.itemService = itemService;
            this.shelfService = shelfService;
            this.categoryService = categoryService;
            this.storeService = storeService;
        }

        public IActionResult Index()
        {
            IEnumerable<Item> items = itemService.GetAll();
            LoadViewBagShelvesList();
            LoadViewBagStoresList();
            return View(items);
        }


        [HttpGet]
        public ActionResult Create()
        {
            // Получите список категорий из вашего сервиса или базы данных
            LoadViewBagShelvesList();
            LoadViewBagCategoriesList();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Item item)
        {

            if (item.ItemName != null)
            {
                item.Shelf = (from shelf in shelfService.GetAll() where shelf.Id == item.ShelfId select shelf).First();
                item.Category = (from category in categoryService.GetAll() where category.Id == item.CategoryId select category).First();

                itemService.Save(item);
                return RedirectToAction("Index");// Перенаправить на экшен Index
            }


            LoadViewBagCategoriesList();
            LoadViewBagShelvesList();

            return View();

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var item = itemService.GetById(id);

            if (item == null)
            {
                return NotFound();
            }

            itemService.Delete(item);

            return RedirectToAction("Index");

        }

        public IActionResult Edit(int id)
        {
            var shelf = itemService.GetById(id);
            if (shelf == null)
            {
                return NotFound();
            }
            LoadViewBagCategoriesList();
            LoadViewBagShelvesList();

            return View("Edit", shelf);
        }

        [HttpPost]
        public IActionResult Update(Item item)
        {
            if (item.ItemName != null)
            {
                item.Shelf = (from shelf in shelfService.GetAll() where shelf.Id == item.ShelfId select shelf).First();
                item.Category = (from category in categoryService.GetAll() where category.Id == item.CategoryId select category).First();


                itemService.UpdateItem(item);
                return RedirectToAction("Index");
            }
            return View("Edit", item);
        }

        private void LoadViewBagShelvesList()
        {
            ViewBag.ShelfList = ViewBagLoader.LoadViewBagList(shelfService.GetAll(), "Id", "ShelfName");

        }

        private void LoadViewBagCategoriesList()
        {
            ViewBag.CategoryList = ViewBagLoader.LoadViewBagList(categoryService.GetAll(), "Id", "CategoryName");
        }

        private void LoadViewBagStoresList()
        {
            ViewBag.StoreList = ViewBagLoader.LoadViewBagList(storeService.GetAll(), "Id", "StoreName");
        }

    }
}

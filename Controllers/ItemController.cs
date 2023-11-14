using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.Design;
using WebInventoryManagement.Models.Data;
using WebInventoryManagement.Services;

namespace WebInventoryManagement.Controllers
{
    public class ItemController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private ItemService itemService;

        public ItemController(ItemService itemService)
        {
            this.itemService = itemService;
        }

        public IActionResult Index()
        {
            IEnumerable<Item> items = itemService.GetAllItems();
            LoadViewBagShelvesList();
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
                item.Shelf = (from shelf in itemService.GetAllShelves() where shelf.Id == item.ShelfId select shelf).First();
                item.Category = (from category in itemService.GetAllCategories() where category.Id == item.CategoryId select category).First();

                itemService.AddItem(item);
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

            itemService.DeleteItem(item);

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
                item.Shelf = (from shelf in itemService.GetAllShelves() where shelf.Id == item.ShelfId select shelf).First();
                item.Category = (from category in itemService.GetAllCategories() where category.Id == item.CategoryId select category).First();


                itemService.UpdateItem(item);
                return RedirectToAction("Index");
            }
            return View("Edit", item);
        }

        private void LoadViewBagShelvesList()
        {
            ViewBag.ShelfList = new SelectList(itemService.GetAllShelves(), "Id", "ShelfName");
        }

        private void LoadViewBagCategoriesList()
        {
            ViewBag.CategoryList = new SelectList(itemService.GetAllCategories(), "Id", "CategoryName");
        }
    }
}

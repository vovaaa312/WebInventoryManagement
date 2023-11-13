namespace WebInventoryManagement.Models.Data
{
    public class Shelf
    {
        public int Id { get; set; }
        public string ShelfName { get; set; }
        public int ItemCount { get; set; }
        public Category Category { get; set; }

        public Store Store { get; set; }

        public int StoreId { get; set; }

        public int CategoryId { get; set; }
        public Shelf(int id, string shelfName, int itemCount, Category category, Store store)
        {
            Id = id;
            ShelfName = shelfName;
            ItemCount = itemCount;
            Category = category;
            Store = store;


            StoreId = store.Id;
            CategoryId = category.Id;
        }
        public Shelf() { }
    }
}

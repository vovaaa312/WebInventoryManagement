namespace WebInventoryManagement.Models.Data
{
    public class Item
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public Shelf Shelf { get; set; }
        public Category Category { get; set; }
        public int ShelfId { get; set; }
        public int CategoryId { get; set; }

        public Store Store { get; set; }

        public int StoreId { get; set; } 


        public Item(int id, string itemName, int price, int quantity, string description, Shelf shelf, Category category)
        {
            Id = id;
            ItemName = itemName;
            Price = price;
            Quantity = quantity;
            Description = description;
            Shelf = shelf;
            Category = category;

            StoreId = shelf?.Store?.Id ?? 0;
            Store = shelf.Store;

            ShelfId = shelf?.Id ?? 0;
            CategoryId = category?.Id ?? 0;
        }

        public Item() { }
    }
}

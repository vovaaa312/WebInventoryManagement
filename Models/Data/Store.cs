namespace WebInventoryManagement.Models.Data
{
    public class Store
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public int ShelfCount { get; set; } = 0;



        public Store(int id, string storeName, int shelfCount)
        {
            Id = id;
            StoreName = storeName;
            ShelfCount = shelfCount;
        }

        public Store()
        {
        }
    }
}

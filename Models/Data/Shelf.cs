namespace WebInventoryManagement.Models.Data
{
    public class Shelf
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int ItemsCount { get; set; } = 0;
        public int Capacity { get; set; }
    }
}

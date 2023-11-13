using System.ComponentModel.DataAnnotations;

namespace WebInventoryManagement.Models.Data
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public Category(int id, string categoryName)
        {
            Id = id;
            CategoryName = categoryName;
        }

        public Category() { }
    }
}

using System.Xml.Linq;

namespace WebInventoryManagement.Models.Data
{
    public class Item
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }

        public Category Category { get; set; }
        public Item(string id, string name, Category category, int price, int quantity, string description)
        {
            Id = id;
            Name = name;
            Price = price;
            Quantity = quantity;
            Description = description;
            Category = category;
        }

        public Item(string id, string name, Category category, int price, int quantity)
            : this(id, name, category, price, quantity, "")
        { }

        //public Item(string id, string name, Category category, int price, int quantity)
        //{
        //    Item(id,name, category, price, quantity, "");
        //    //Id = id;
        //    //Name = name;
        //    //Category = category;
        //    //Price = price;
        //    //Description = "";
        //    //Quantity = quantity;
        //}

        public override bool Equals(object? obj)
        {
            return obj is Item item &&
            Id == item.Id &&
                   Name == item.Name &&
                   Price == item.Price &&
                   Quantity == item.Quantity &&
                   Description == item.Description;
        }

        public override int GetHashCode()
        {
            return 32 * this.Id.GetHashCode() * this.Name.GetHashCode() * this.Price.GetHashCode() *
                this.Category.GetHashCode() * this.Quantity.GetHashCode() * this.Description.GetHashCode();
        }

        public override string? ToString()
        {
            return $"{Id} {Name} {Category} {Price} {Quantity} {Description}";
        }


    }
}

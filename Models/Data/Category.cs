using System.Collections;

namespace WebInventoryManagement.Models.Data
{
    public enum Category
    {
        Meat, Fish, Dairy, Vegetables, Fruits, Freezer, Alcohol, DriedGoods, Snacks, Care, None
    }

    public static class CategoryInfo
    {
        public const int Count = 11;

        public static IEnumerable Items
        {
            get
            {
                List<Category> clubList = new();
                clubList.Add(Category.None);
                clubList.Add(Category.Meat);
                clubList.Add(Category.Fish);
                clubList.Add(Category.Dairy);
                clubList.Add(Category.Vegetables);
                clubList.Add(Category.Fruits);
                clubList.Add(Category.Freezer);
                clubList.Add(Category.Alcohol);
                clubList.Add(Category.DriedGoods);
                clubList.Add(Category.Snacks);
                clubList.Add(Category.Care);

                return clubList;

            }
        }
    }
}

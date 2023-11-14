using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Data;
using WebInventoryManagement.Models.Data;

namespace WebInventoryManagement.Services
{
    public class ItemService
    {
        private readonly string _connectionString;
        public ItemService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("OracleConnection");
        }

        public IEnumerable<Item> GetAllItems() {

            OracleConnection con = new OracleConnection(_connectionString);
            con.Open();

            OracleCommand getAllItemsCommand = new OracleCommand();
            getAllItemsCommand.Connection = con;
            getAllItemsCommand.CommandText = "SELECT * FROM ITEMS";
            OracleDataReader reader = getAllItemsCommand.ExecuteReader();

            List<Item> tmp = new();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Item item = fillItemData(reader);
                    tmp.Add(item);
                }
            }

            con.Close();

            return tmp;
        }
        public IEnumerable<Category> GetAllCategories()
        {
            OracleConnection con = new OracleConnection(_connectionString);
            con.Open();

            OracleCommand getAllStoresCommand = new OracleCommand();
            getAllStoresCommand.Connection = con;
            getAllStoresCommand.CommandText = "SELECT * FROM categories";
            OracleDataReader reader = getAllStoresCommand.ExecuteReader();

            List<Category> tmp = new();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["category_id"]);
                    string ctgName = reader["category_name"].ToString();
                    tmp.Add(new Category(id, ctgName));
                }
            }

            con.Close();

            return tmp;
        }
        public IEnumerable<Shelf> GetAllShelves()
        {
            OracleConnection con = new OracleConnection(_connectionString);
            con.Open();

            OracleCommand getAllShelvesCommand = new OracleCommand();
            getAllShelvesCommand.Connection = con;
            getAllShelvesCommand.CommandText = "SELECT * FROM SHELVES";
            OracleDataReader reader = getAllShelvesCommand.ExecuteReader();

            List<Shelf> tmp = new();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Shelf shelf = fillShelfData(reader);
                    tmp.Add(shelf);
                }
            }

            con.Close();

            return tmp;
        }

        public void AddItem(Item item)
        {
            OracleConnection con = new(_connectionString);

            con.Open();
            OracleCommand insertStoreCommand = new("INSERT_ITEM", con);
            insertStoreCommand.CommandType = System.Data.CommandType.StoredProcedure;

            insertStoreCommand.Parameters.Add("ITEM_NAME_P", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = item.ItemName;
            insertStoreCommand.Parameters.Add("PRICE_P", OracleDbType.Int32, System.Data.ParameterDirection.Input).Value = item.Price;
            insertStoreCommand.Parameters.Add("QUANTITY_P", OracleDbType.Int32, System.Data.ParameterDirection.Input).Value = item.Quantity;
            insertStoreCommand.Parameters.Add("DESCRIPTION_P", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = item.Description;
            insertStoreCommand.Parameters.Add("CATEGORY_ID_P", OracleDbType.Int32, System.Data.ParameterDirection.Input).Value = item.CategoryId;
            insertStoreCommand.Parameters.Add("SHELF_ID_P", OracleDbType.Int32, System.Data.ParameterDirection.Input).Value = item.ShelfId;

            insertStoreCommand.ExecuteNonQuery();

            con.Close();
        }
        public void DeleteItem(Item item)
        {
            OracleConnection con = new(_connectionString);
            con.Open();
            OracleCommand deleteItemCommand = new("DELETE_ITEM_BY_ID", con);
            deleteItemCommand.CommandType = System.Data.CommandType.StoredProcedure;

            deleteItemCommand.Parameters.Add("ITEM_ID_P", OracleDbType.Int16, ParameterDirection.Input).Value = item.Id;

            deleteItemCommand.ExecuteNonQuery();

            con.Close();
        }

        public void UpdateItem(Item item)
        {

            OracleConnection con = new(_connectionString);
            con.Open();
            OracleCommand updateStoreCommand = new("UPDATE_ITEM", con);
            updateStoreCommand.CommandType = System.Data.CommandType.StoredProcedure;

            updateStoreCommand.Parameters.Add("OLD_ITEM_ID_P", OracleDbType.Int32, ParameterDirection.Input).Value = item.Id;
            updateStoreCommand.Parameters.Add("ITEM_NAME_P", OracleDbType.Varchar2, ParameterDirection.Input).Value = item.ItemName;
            updateStoreCommand.Parameters.Add("PRICE_P", OracleDbType.Int32, ParameterDirection.Input).Value = item.Price;
            updateStoreCommand.Parameters.Add("QUANTITY_P", OracleDbType.Int32, ParameterDirection.Input).Value = item.Quantity;
            updateStoreCommand.Parameters.Add("DESCRIPTION_P", OracleDbType.Varchar2, ParameterDirection.Input).Value = item.Description;
            updateStoreCommand.Parameters.Add("CATEGORY_ID_P", OracleDbType.Int32, ParameterDirection.Input).Value = item.CategoryId;
            updateStoreCommand.Parameters.Add("SHELF_ID_P", OracleDbType.Int32, ParameterDirection.Input).Value = item.ShelfId;


            updateStoreCommand.ExecuteNonQuery();

            con.Close();
        }
        
        public Item GetById(int id)
        {
            OracleConnection con = new OracleConnection(_connectionString);
            con.Open();
            OracleCommand getByIdCommand = new OracleCommand();
            getByIdCommand.Connection = con;
            getByIdCommand.CommandText = $"SELECT * FROM ITEMS WHERE ITEMS.ITEM_ID = {id}";
            getByIdCommand.Parameters.Add(new OracleParameter("itemId", id));
            OracleDataReader reader = getByIdCommand.ExecuteReader();

            Item Item = new Item();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Item.Id = id;
                    Item.ItemName = reader["ITEM_NAME"].ToString();
                    Item.Price = Convert.ToInt32(reader["PRICE"]);
                    Item.Quantity = Convert.ToInt32(reader["QUANTITY"]);
                    Item.Description = reader["DESCRIPTION"].ToString();

                    Item.Category = GetCategoryById(Convert.ToInt32(reader["category_id_fk"]));
                    Item.Shelf = getShelfById(Convert.ToInt32(reader["shelf_id_fk"]));
                }
            }
            if (Item == null) throw new NullReferenceException();
            return Item;
        }
        private Category GetCategoryById(int id)
        {
            OracleConnection con = new OracleConnection(_connectionString);
            con.Open();

            OracleCommand getCategory = new OracleCommand();
            getCategory.Connection = con;
            getCategory.CommandText = $"select * from CATEGORIES where CATEGORIES.CATEGORY_ID = {id}";
            OracleDataReader reader = getCategory.ExecuteReader();

            Category cat = new();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    cat.Id = id;
                    cat.CategoryName = reader["CATEGORY_NAME"].ToString();

                }
            }
            return cat;
        }
        private Shelf getShelfById(int id)
        {
            OracleConnection con = new OracleConnection(_connectionString);
            con.Open();
            OracleCommand getByIdCommand = new OracleCommand();
            getByIdCommand.Connection = con;
            getByIdCommand.CommandText = $"SELECT * FROM SHELVES WHERE SHELVES.SHELF_ID = {id}";
            getByIdCommand.Parameters.Add(new OracleParameter("storeId", id));
            OracleDataReader reader = getByIdCommand.ExecuteReader();

            Shelf shelf = new Shelf();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    shelf.Id = id;
                    shelf.ShelfName = reader["shelf_name"].ToString();
                    shelf.ItemCount = Convert.ToInt32(reader["item_count"]);
                    shelf.Category = GetCategoryById(Convert.ToInt32(reader["category_id_fk"]));
                    shelf.Store = getStoreById(Convert.ToInt32(reader["store_id_fk"]));
                }
            }
            if (shelf == null) throw new NullReferenceException();
            return shelf;
        }
        private Store getStoreById(int id)
        {
            OracleConnection con = new OracleConnection(_connectionString);
            con.Open();

            OracleCommand getStore = new OracleCommand();
            getStore.Connection = con;
            getStore.CommandText = $"select * from STORES where STORES.STORE_ID= {id}";
            OracleDataReader reader = getStore.ExecuteReader();

            Store store = new();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    store.Id = id;
                    store.StoreName = reader["store_name"].ToString();
                    store.ShelfCount = Convert.ToInt32(reader["shelf_count"]);
                }
            }
            return store;
        }
        private Item fillItemData(OracleDataReader reader)
        {
            int id = Convert.ToInt32(reader["ITEM_ID"]);
            string itemName = reader["ITEM_NAME"].ToString();
            int price = Convert.ToInt32(reader["PRICE"]);
            int quantity = Convert.ToInt32(reader["QUANTITY"]);

            string description = reader["DESCRIPTION"].ToString();

            Category category = GetCategoryById(Convert.ToInt32(reader["CATEGORY_ID_FK"]));
            Shelf shelf = getShelfById(Convert.ToInt32(reader["SHELF_ID_FK"]));

            return new Item(id, itemName, price, quantity, description, shelf, category);
        }
        private Shelf fillShelfData(OracleDataReader reader)
        {
            int id = Convert.ToInt32(reader["shelf_id"]);
            string shelfName = reader["shelf_name"].ToString();
            int itemCount = Convert.ToInt32(reader["item_count"]);
            Category category = GetCategoryById(Convert.ToInt32(reader["CATEGORY_ID_FK"]));
            Store store = getStoreById(Convert.ToInt32(reader["STORE_ID_FK"]));

            return new Shelf(id, shelfName, itemCount, category, store);
        }

        
    }
}

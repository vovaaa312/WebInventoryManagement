using Oracle.ManagedDataAccess.Client;
using System.Data;
using WebInventoryManagement.Models.Data;
using static System.Formats.Asn1.AsnWriter;

namespace WebInventoryManagement.Services
{
    public class ShelfService
    {
        private readonly string _connectionString;
        public ShelfService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("OracleConnection");
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

        public IEnumerable<Store> GetAllStores() {
            OracleConnection con = new OracleConnection(_connectionString);
            con.Open();

            OracleCommand getAllStoresCommand = new OracleCommand();
            getAllStoresCommand.Connection = con;
            getAllStoresCommand.CommandText = "SELECT * FROM STORES";
            OracleDataReader reader = getAllStoresCommand.ExecuteReader();

            List<Store> tmp = new();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Store store = fillStoreData(reader);
                    tmp.Add(store);
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

        private Store fillStoreData(OracleDataReader reader)
        {
            int id = Convert.ToInt32(reader["STORE_ID"]);
            string storeName = reader["STORE_NAME"].ToString();
            int shelvfCount = Convert.ToInt32(reader["SHELF_COUNT"]);

            return new Store(id, storeName, shelvfCount);

        }

        public Shelf GetById(int id)
        {

            // get store by id from database logic

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
        public void AddShelf(Shelf shelf)
        {
            OracleConnection con = new(_connectionString);

            con.Open();
            OracleCommand insertStoreCommand = new("INSERT_SHELF", con);
            insertStoreCommand.CommandType = System.Data.CommandType.StoredProcedure;

            insertStoreCommand.Parameters.Add("SHELF_NAME_P", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = shelf.ShelfName;
            insertStoreCommand.Parameters.Add("CATEGORY_ID_P", OracleDbType.Int32, System.Data.ParameterDirection.Input).Value = shelf.CategoryId;
            insertStoreCommand.Parameters.Add("STORE_ID_P", OracleDbType.Int32, System.Data.ParameterDirection.Input).Value = shelf.StoreId;

            insertStoreCommand.ExecuteNonQuery();

            con.Close();
        }

        public void UpdateShelf(Shelf shelf)
        {
            OracleConnection con = new(_connectionString);
            con.Open();
            OracleCommand updateStoreCommand = new("UPDATE_SHELF", con);
            updateStoreCommand.CommandType = System.Data.CommandType.StoredProcedure;

            updateStoreCommand.Parameters.Add("OLD_SHELF_ID_P", OracleDbType.Int16, ParameterDirection.Input).Value = shelf.Id;
            updateStoreCommand.Parameters.Add("NEW_SHELF_NAME_P", OracleDbType.Varchar2, ParameterDirection.Input).Value = shelf.ShelfName;
            updateStoreCommand.Parameters.Add("NEW_SHELF_CATEGORY_P", OracleDbType.Varchar2, ParameterDirection.Input).Value = shelf.CategoryId;


            updateStoreCommand.ExecuteNonQuery();

            con.Close();
        }
        public void DeleteShelf(Shelf shelf)
        {
            OracleConnection con = new(_connectionString);
            con.Open();
            OracleCommand deleteStoreCommand = new("DELETE_SHELF_BY_ID", con);
            deleteStoreCommand.CommandType = System.Data.CommandType.StoredProcedure;

            deleteStoreCommand.Parameters.Add("SHELF_ID_P", OracleDbType.Int16, ParameterDirection.Input).Value = shelf.Id;

            deleteStoreCommand.ExecuteNonQuery();

            con.Close();
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



    }
}

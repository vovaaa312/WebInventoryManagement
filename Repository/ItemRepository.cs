using Oracle.ManagedDataAccess.Client;
using System.Data;
using WebInventoryManagement.Models.Data;

namespace WebInventoryManagement.Repository
{
    public class ItemRepository
    {

        private readonly string _connectionString;

        private ShelfRepository shelfRepository;
        private CategoryRepository categoryRepository;

        public ItemRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("OracleConnection");
            shelfRepository = new ShelfRepository(configuration);
            categoryRepository = new CategoryRepository(configuration);
        }

        public IEnumerable<Item> GetAll()
        {

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

                    Item.Category = categoryRepository.GetById(Convert.ToInt32(reader["category_id_fk"]));
                    Item.Shelf = shelfRepository.GetById(Convert.ToInt32(reader["shelf_id_fk"]));
                }
            }
            if (Item == null) throw new NullReferenceException();
            return Item;
        }

        public void Save(Item item) {
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

        public void Delete(Item item)
        {
            OracleConnection con = new(_connectionString);
            con.Open();
            OracleCommand deleteItemCommand = new("DELETE_ITEM_BY_ID", con);
            deleteItemCommand.CommandType = System.Data.CommandType.StoredProcedure;

            deleteItemCommand.Parameters.Add("ITEM_ID_P", OracleDbType.Int16, ParameterDirection.Input).Value = item.Id;

            deleteItemCommand.ExecuteNonQuery();

            con.Close();
        }

        public void Update(Item item) 
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
       
        
        
        
        private Item fillItemData(OracleDataReader reader)
        {
            int id = Convert.ToInt32(reader["ITEM_ID"]);
            string itemName = reader["ITEM_NAME"].ToString();
            int price = Convert.ToInt32(reader["PRICE"]);
            int quantity = Convert.ToInt32(reader["QUANTITY"]);

            string description = reader["DESCRIPTION"].ToString();

            Category category = categoryRepository.GetById(Convert.ToInt32(reader["CATEGORY_ID_FK"]));
            Shelf shelf = shelfRepository.GetById(Convert.ToInt32(reader["SHELF_ID_FK"]));

            return new Item(id, itemName, price, quantity, description, shelf, category);
        }


    }
}

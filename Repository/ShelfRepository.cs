using Oracle.ManagedDataAccess.Client;
using System.Data;
using WebInventoryManagement.Models.Data;

namespace WebInventoryManagement.Repository
{
    public class ShelfRepository
    {
        private readonly string _connectionString;

        private CategoryRepository categoryRepository;
        private StoreRepository storeRepository;

        public ShelfRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("OracleConnection");
            categoryRepository = new CategoryRepository(configuration);
            storeRepository = new StoreRepository(configuration);
        }

        public IEnumerable<Shelf> GetAll()
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
        public Shelf GetById(int id)
        {

            // get shelf by id from database logic

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
                    shelf.Category = categoryRepository.GetById(Convert.ToInt32(reader["CATEGORY_ID_FK"]));
                    shelf.Store = storeRepository.GetById(Convert.ToInt32(reader["STORE_ID_FK"]));
                }
            }
            if (shelf == null) throw new NullReferenceException();
            return shelf;
        }

        public void Save(Shelf shelf)
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

        public void Update(Shelf shelf)
        {
            OracleConnection con = new(_connectionString);
            con.Open();
            OracleCommand updateShelfCommand = new("UPDATE_SHELF", con);
            updateShelfCommand.CommandType = System.Data.CommandType.StoredProcedure;

            updateShelfCommand.Parameters.Add("OLD_SHELF_ID_P", OracleDbType.Int16, ParameterDirection.Input).Value = shelf.Id;
            updateShelfCommand.Parameters.Add("NEW_SHELF_NAME_P", OracleDbType.Varchar2, ParameterDirection.Input).Value = shelf.ShelfName;
            updateShelfCommand.Parameters.Add("NEW_SHELF_CATEGORY_P", OracleDbType.Varchar2, ParameterDirection.Input).Value = shelf.CategoryId;


            updateShelfCommand.ExecuteNonQuery();

            con.Close();
        }

        public void Delete(Shelf shelf)
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

            Category category = categoryRepository.GetById(Convert.ToInt32(reader["CATEGORY_ID_FK"]));
            //Category category = GetCategoryById(Convert.ToInt32(reader["CATEGORY_ID_FK"]));
            Store store = storeRepository.GetById(Convert.ToInt32(reader["STORE_ID_FK"]));
            // Store store = getStoreById(Convert.ToInt32(reader["STORE_ID_FK"]));

            return new Shelf(id, shelfName, itemCount, category, store);
        }

    }
}

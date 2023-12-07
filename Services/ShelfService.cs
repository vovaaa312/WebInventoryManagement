using Oracle.ManagedDataAccess.Client;
using System.Data;
using WebInventoryManagement.Models.Data;
using WebInventoryManagement.Repository;
using static System.Formats.Asn1.AsnWriter;

namespace WebInventoryManagement.Services
{
    public class ShelfService
    {

        private ShelfRepository shelfRepository;
        public ShelfService(IConfiguration configuration)
        {
            //_connectionString = configuration.GetConnectionString("OracleConnection");
            shelfRepository = new ShelfRepository(configuration);
        }

        public IEnumerable<Shelf> GetAll()
        {
            //OracleConnection con = new OracleConnection(_connectionString);
            //con.Open();

            //OracleCommand getAllShelvesCommand = new OracleCommand();
            //getAllShelvesCommand.Connection = con;
            //getAllShelvesCommand.CommandText = "SELECT * FROM SHELVES";
            //OracleDataReader reader = getAllShelvesCommand.ExecuteReader();

            //List<Shelf> tmp = new();
            //if (reader.HasRows)
            //{
            //    while (reader.Read())
            //    {
            //        Shelf shelf = fillShelfData(reader);
            //        tmp.Add(shelf);
            //    }
            //}

            //con.Close();

            //return tmp;

            return shelfRepository.GetAll();

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


            return shelfRepository.GetById(id);
        }
        public void Save(Shelf shelf)
        {
            shelfRepository.Save(shelf);
        }

        public void Update(Shelf shelf)
        {
            shelfRepository.Update(shelf);
        }
        public void Delete(Shelf shelf)
        {
          shelfRepository.Delete(shelf);
        }
       




    }
}

using Oracle.ManagedDataAccess.Client;
using WebInventoryManagement.Models.Data;

namespace WebInventoryManagement.Repository
{
    public class CategoryRepository
    {
        private readonly string _connectionString;

        public CategoryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("OracleConnection");
        }

        public IEnumerable<Category> GetAll()
        {
            OracleConnection con = new OracleConnection(_connectionString);
            con.Open();

            OracleCommand getAllCategoriesCommand = new OracleCommand();
            getAllCategoriesCommand.Connection = con;
            getAllCategoriesCommand.CommandText = "SELECT * FROM CATEGORIES";
            OracleDataReader reader = getAllCategoriesCommand.ExecuteReader();

            List<Category> tmp = new();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Category category = new Category
                    {
                        Id = Convert.ToInt32(reader["CATEGORY_ID"]),
                        CategoryName = reader["CATEGORY_NAME"].ToString()
                    };

                    tmp.Add(category);
                }
            }

            con.Close();

            return tmp;
        }

        public Category GetById(int id)
        {
            Category category = null;
            // get store by id from database logic

            OracleConnection con = new OracleConnection(_connectionString);
            con.Open();
            OracleCommand getByIdCommand = new OracleCommand();
            getByIdCommand.Connection = con;
            getByIdCommand.CommandText = $"SELECT * FROM CATEGORIES WHERE CATEGORIES.CATEGORY_ID = {id}";
            getByIdCommand.Parameters.Add(new OracleParameter("categoryId", id));
            OracleDataReader reader = getByIdCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                     category = new Category
                    {
                        Id = Convert.ToInt32(reader["CATEGORY_ID"]),
                        CategoryName = reader["CATEGORY_NAME"].ToString()
                    };
                }
            }
            if (category == null) throw new NullReferenceException();
            return category;
        }
   
        
    }
}

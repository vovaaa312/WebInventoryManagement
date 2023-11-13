using Oracle.ManagedDataAccess.Client;
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
        public void AddItem(Store item)
        {

        }

    }
}

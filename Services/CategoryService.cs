using Oracle.ManagedDataAccess.Client;
using System.Data;
using WebInventoryManagement.Models.Data;
using WebInventoryManagement.Repository;

namespace WebInventoryManagement.Services
{
    public class CategoryService
    {

        private CategoryRepository CategoryRepository;
        public CategoryService(IConfiguration configuration)
        {
            CategoryRepository = new CategoryRepository(configuration);
        }

        public IEnumerable<Category> GetAll()
        {
            return CategoryRepository.GetAll();
        }

        private Category GetById(int id)
        {
            return CategoryRepository.GetById(id);
        }



    }

}

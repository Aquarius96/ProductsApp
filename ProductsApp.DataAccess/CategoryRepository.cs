using ProductsApp.Logic.Repositories;
using ProductsApp.Models;

namespace ProductsApp.DataAccess
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(DataContext db) : base(db)
        {
        }
    }
}

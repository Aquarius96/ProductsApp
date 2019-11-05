using System.Linq;
using ProductsApp.Logic.Repositories;
using ProductsApp.Models;
using Z.EntityFramework.Plus;

namespace ProductsApp.DataAccess
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(DataContext db) : base(db)
        {
        }

        public override void Delete(Category category)
        {
            category.IsActive = false;

            _db.Products.Where(p => p.CategoryId == category.Id)
                .Update(p => new Product() { IsActive = false });
        }
    }
}

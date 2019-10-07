using ProductsApp.Logic.Repositories;
using ProductsApp.Models;

namespace ProductsApp.DataAccess
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(DataContext db)
            :base(db)
        { }        
    }
}

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductsApp.Logic.Repositories;
using ProductsApp.Models;

namespace ProductsApp.DataAccess
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(DataContext db)
            :base(db)
        { }

        public void Update(Product product)
        {
            _db.Entry(product).State = EntityState.Modified;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using ProductsApp.Logic.Repositories;
using ProductsApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsApp.DataAccess
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(DataContext db)
            :base(db)
        { }

        public override async Task<IEnumerable<Product>> GetAllActive()
        {
            return await _db.Products.Where(m => m.IsActive)
                .Include(m => m.Category)
                .ToListAsync();
        }

        public override async Task<Product> GetById(int id)
        {
            return await _db.Products.Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}

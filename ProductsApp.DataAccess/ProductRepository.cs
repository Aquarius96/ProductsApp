using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductsApp.Logic.Repositories;
using ProductsApp.Logic.Services.Interfaces;
using ProductsApp.Models;

namespace ProductsApp.DataAccess
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(DataContext db, IDateService dateService)
            :base(db, dateService)
        { }        
    }
}

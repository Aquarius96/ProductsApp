using ProductsApp.Models;
using System.Threading.Tasks;

namespace ProductsApp.Logic.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);
    }
}

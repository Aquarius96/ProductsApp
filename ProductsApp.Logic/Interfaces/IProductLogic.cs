using ProductsApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductsApp.Logic.Interfaces
{
    public interface IProductLogic : ILogic
    {
        Task<Result<Product>> Create(Product product);
        Task<Result<Product>> GetById(int id);
        Task<Result<IEnumerable<Product>>> GetAllActive();
        Task<Result<Product>> Remove(Product product);
        Task<Result<Product>> Update(Product product);
    }
}

using ProductsApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductsApp.Logic.Repositories
{
    public interface IRepository<T> where T : BaseModel
    {
        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetAllActive();
        Task Add(T model);
        void Delete(T model);
        Task SaveChanges();
    }
}

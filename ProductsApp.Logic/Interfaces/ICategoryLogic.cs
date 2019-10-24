using ProductsApp.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductsApp.Logic.Interfaces
{
    public interface ICategoryLogic : ILogic
    {
        Task<Result<Category>> Create(Category category);
        Task<Result<Category>> GetById(int id);
        Task<Result<IEnumerable<Category>>> GetAllActive();
        Task<Result<Category>> Remove(Category category);
        Task<Result<Category>> Update(Category category);
    }
}

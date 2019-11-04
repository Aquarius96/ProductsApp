using System.Threading.Tasks;

namespace ProductsApp.Logic.Services.Interfaces
{
    public interface IDatabaseRestoreService : IService
    {
        Task<Result> Restore();
    }
}
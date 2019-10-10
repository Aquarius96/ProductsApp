using System;

namespace ProductsApp.Logic.Services.Interfaces
{
    public interface IDateService : IService
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
    }
}

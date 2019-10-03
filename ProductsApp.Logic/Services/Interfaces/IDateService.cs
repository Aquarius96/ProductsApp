using System;

namespace ProductsApp.Logic.Services.Interfaces
{
    public interface IDateService
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
    }
}

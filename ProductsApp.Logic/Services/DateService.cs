using System;
using ProductsApp.Logic.Services.Interfaces;

namespace ProductsApp.Logic.Services
{
    public class DateService : IDateService
    {
        public DateTime Now => DateTime.Now;

        public DateTime UtcNow => DateTime.UtcNow;
    }
}

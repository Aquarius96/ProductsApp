using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProductsApp.Logic.Services;

namespace ProductsApp.DataAccess
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-09QVQ5G;Initial Catalog=ProductsApp;Integrated Security=SSPI;User ID=sh");

            var dateService = new DateService();
            return new DataContext(optionsBuilder.Options, dateService);
        }
    }
}

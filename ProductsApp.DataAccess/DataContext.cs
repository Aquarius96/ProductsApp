using Microsoft.EntityFrameworkCore;
using ProductsApp.Logic.Services.Interfaces;
using ProductsApp.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProductsApp.DataAccess
{
    public class DataContext : DbContext
    {
        private readonly IDateService _dateService;
        public DataContext(DbContextOptions<DataContext> options, IDateService dateService)
            : base(options)
        {
            _dateService = dateService;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        private string _userName;

        public string UserName
        {
            get
            {
                return _userName ?? "System";
            }
            set
            {
                _userName = value;
            }
        }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ChangeTracker.DetectChanges();

            var entries = ChangeTracker.Entries();

            foreach(var entity in entries.Where(entry => entry.State == EntityState.Modified))
            {
                if (!(entity.Entity is BaseModel baseObject))
                {
                    continue;
                }

                baseObject.UpdatedDate = _dateService.Now;
                baseObject.UpdatedUser = UserName;
            }

            foreach(var entity in entries.Where(entry => entry.State == EntityState.Added))
            {
                if (!(entity.Entity is BaseModel baseObject))
                {
                    continue;
                }

                baseObject.CreatedDate = _dateService.Now;
                baseObject.UpdatedDate = _dateService.Now;
                baseObject.CreatedUser = UserName;
                baseObject.UpdatedUser = UserName;
            }

            return await base.SaveChangesAsync(true, cancellationToken);
        }
    }
}

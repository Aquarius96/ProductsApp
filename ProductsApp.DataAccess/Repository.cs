using Microsoft.EntityFrameworkCore;
using ProductsApp.Logic.Repositories;
using ProductsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsApp.DataAccess
{
    public abstract class Repository<T> : IRepository<T> where T : BaseModel
    {
        private readonly Lazy<DataContext> _db;
        protected DataContext Db => _db.Value;

        protected Repository(Lazy<DataContext> db)
        {
            _db = db;
        }

        public async Task Add(T model)
        {
            await Db.Set<T>().AddAsync(model);
        }

        public void Delete(T model)
        {
            Db.Set<T>().Remove(model);
        }

        public async Task<IEnumerable<T>> GetAllActive()
        {
            return await Db.Set<T>().Where(m => m.IsActive)
                .ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await Db.Set<T>().FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task SaveChanges()
        {
            await Db.SaveChangesAsync();
        }
    }
}

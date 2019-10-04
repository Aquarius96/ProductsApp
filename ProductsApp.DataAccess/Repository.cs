﻿using Microsoft.EntityFrameworkCore;
using ProductsApp.Logic.Repositories;
using ProductsApp.Logic.Services.Interfaces;
using ProductsApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsApp.DataAccess
{
    public abstract class Repository<T> : IRepository<T> where T : BaseModel
    {
        protected readonly DataContext _db;
        protected readonly IDateService _dateService;

        protected Repository(DataContext db, IDateService dateService)
        {
            _db = db;
            _dateService = dateService;
        }

        public async Task Add(T model)
        {
            model.CreatedDate = _dateService.UtcNow;
            await _db.Set<T>().AddAsync(model);
        }

        public void Delete(T model)
        {
            _db.Set<T>().Remove(model);
        }

        public async Task<IEnumerable<T>> GetAllActive()
        {
            return await _db.Set<T>().Where(m => m.IsActive)
                .ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _db.Set<T>().FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task SaveChanges()
        {
            await _db.SaveChangesAsync();
        }
    }
}

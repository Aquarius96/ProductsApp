using ProductsApp.Logic.Interfaces;
using ProductsApp.Logic.Repositories;
using ProductsApp.Logic.Services.Interfaces;
using ProductsApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductsApp.Logic.Products
{
    public class ProductLogic : IProductLogic
    {
        private readonly IProductRepository _repository;
        private readonly IDateService _dateService;

        public ProductLogic(IProductRepository repository,
            IDateService dateService)
        {
            _repository = repository;
            _dateService = dateService;
        }
        public async Task<Result<Product>> Create(Product product)
        {
            if(product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            product.CreationDate = _dateService.UtcNow;

            await _repository.Add(product);
            await _repository.SaveChanges();

            return Result.Ok(product);
        }

        public async Task<Result<IEnumerable<Product>>> GetAllActive()
        {
            var products = await _repository.GetAllActive();
            return Result.Ok(products);
        }

        public async Task<Result<Product>> GetById(int id)
        {
            var product = await _repository.GetById(id);

            if(product == null)
            {
                return Result.Error<Product>($"There is no Product with id: {id}");
            }

            return Result.Ok(product);
        }

        public async Task<Result<Product>> Remove(Product product)
        {
            if(product == null)
            {
                throw new ArgumentException(nameof(product));
            }

            _repository.Delete(product);
            await _repository.SaveChanges();
            return Result.Ok(product);
        }
    }
}

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
        private readonly ICustomValidatorFactory _validatorFactory;

        public ProductLogic(IProductRepository repository,
            ICustomValidatorFactory validatorFactory)
        {
            _repository = repository;
            _validatorFactory = validatorFactory;
        }
        public async Task<Result<Product>> Create(Product product)
        {
            if(product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            var validator = _validatorFactory.Create<Product>();
            var validationResult = await validator.ValidateAsync(product);
            if (!validationResult.IsValid)
            {
                return Result.Error<Product>(validationResult.Errors);
            }

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

        public async Task<Result> Remove(Product product)
        {
            if(product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            _repository.Delete(product);
            await _repository.SaveChanges();
            return Result.Ok();
        }

        public async Task<Result<Product>> Update(Product product)
        {
            if(product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            var validator = _validatorFactory.Create<Product>();
            var validationResult = await validator.ValidateAsync(product);
            if (!validationResult.IsValid)
            {
                return Result.Error<Product>(validationResult.Errors);
            }

            await _repository.SaveChanges();
            return Result.Ok(product);
        }
    }
}

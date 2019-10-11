using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductsApp.Logic.Interfaces;
using ProductsApp.Logic.Repositories;
using ProductsApp.Logic.Services.Interfaces;
using ProductsApp.Models;

namespace ProductsApp.Logic.Categories
{
    public class CategoryLogic : ICategoryLogic
    {
        private readonly ICategoryRepository _repository;
        private readonly ICustomValidatorFactory _validatorFactory;

        public CategoryLogic(ICategoryRepository repository,
            ICustomValidatorFactory validatorFactory)
        {
            _repository = repository;
            _validatorFactory = validatorFactory;
        }
        public async Task<Result<Category>> Create(Category category)
        {
            if(category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            var validator = _validatorFactory.Create<Category>();
            var validationResult = await validator.ValidateAsync(category);
            if (!validationResult.IsValid)
            {
                return Result.Error<Category>(validationResult.Errors);
            }

            await _repository.Add(category);
            await _repository.SaveChanges();

            return Result.Ok(category);
        }

        public async Task<Result<IEnumerable<Category>>> GetAllActive()
        {
            var categories = await _repository.GetAllActive();
            return Result.Ok(categories);
        }

        public async Task<Result<Category>> GetById(int id)
        {
            var category = await _repository.GetById(id);

            if(category == null)
            {
                return Result.Error<Category>($"There is no category with id: {id}");
            }
            return Result.Ok(category);
        }

        public async Task<Result<Category>> Remove(Category category)
        {
            if(category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            _repository.Delete(category);
            await _repository.SaveChanges();
            return Result.Ok(category);
        }

        public async Task<Result<Category>> Update(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            var validator = _validatorFactory.Create<Category>();
            var validationResult = await validator.ValidateAsync(category);
            if (!validationResult.IsValid)
            {
                return Result.Error<Category>(validationResult.Errors);
            }

            await _repository.SaveChanges();
            return Result.Ok(category);
        }
    }
}

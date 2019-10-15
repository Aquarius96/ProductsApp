using AutoMapper;
using ProductsApp.Logic.Repositories;
using ProductsApp.Models;
using ProductsApp.WebApi.Dto;

namespace ProductsApp.WebApi.Mappers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductForCreationDto, Product>();
            CreateMap<Product, ProductDto>()
                .ForMember(m => m.CategoryName,
                    opt => opt.MapFrom(p => p.Category.Name));
            CreateMap<ProductForCreationDto, ProductDto>()
                .ForMember(m => m.CategoryName,
                    opt => opt.MapFrom<CategoryResolver>());
        }
    }

    public class CategoryResolver : IValueResolver<ProductForCreationDto, ProductDto, string>
    {
        private ICategoryRepository _repository;

        public CategoryResolver(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public string Resolve(ProductForCreationDto source, ProductDto destination, string destMember, ResolutionContext context)
        {
            return _repository.GetById(source.CategoryId).Result.Name;
        }
    }
}

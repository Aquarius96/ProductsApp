using AutoMapper;
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
        }
    }
}

using AutoMapper;
using ProductsApp.Models;
using ProductsApp.WebApi.Dto;

namespace ProductsApp.WebApi.Mappers
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryDto, Category>()
                .ReverseMap();
        }
    }
}

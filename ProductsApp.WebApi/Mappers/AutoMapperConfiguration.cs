using AutoMapper;

namespace ProductsApp.WebApi.Mappers
{
    public class AutoMapperConfiguration : IAutoMapperConfiguration
    {
        public IMapper Configure()
        {
            var assembly = typeof(AutoMapperConfiguration).Assembly;

            var mappingConfig = new MapperConfiguration(mc => { mc.AddMaps(assembly); });

            var mapper = mappingConfig.CreateMapper();
            return mapper;
        }
    }
}

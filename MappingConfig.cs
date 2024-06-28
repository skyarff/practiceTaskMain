using AutoMapper;
using StockService.Models;
using StockService.Models.dto;

namespace StockService
{
    public class MappingConfig
    {
        public static MapperConfiguration RegistrMaps()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<Employee, EmployeeDto>().ReverseMap();
                config.CreateMap<Company, CompanyDto>().ReverseMap();
                config.CreateMap<Provider, ProviderDto>().ReverseMap();
                config.CreateMap<Stock, StockDto>().ReverseMap();
                config.CreateMap<ProductCategory, ProductCategoryDto>().ReverseMap();
            });
        }
    }
}

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
                config.CreateMap<Company, CompanyDto>();
                config.CreateMap<CompanyDto, Company>()
                    .ForMember(dest => dest.CompanyId, opt => opt.Ignore());

                config.CreateMap<Stock, StockDto>();
                config.CreateMap<StockDto, Stock>()
                    .ForMember(dest => dest.StockId, opt => opt.Ignore());

                config.CreateMap<ProductCategory, ProductCategoryDto>();
                config.CreateMap<ProductCategoryDto, ProductCategory>()
                    .ForMember(dest => dest.ProductCategoryId, opt => opt.Ignore());

                config.CreateMap<Employee, EmployeeDto>();
                config.CreateMap<EmployeeDto, Employee>()
                    .ForMember(dest => dest.EmployeeId, opt => opt.Ignore());

                config.CreateMap<StorageLocation, StorageLocationDto>();
                config.CreateMap<StorageLocationDto, StorageLocation>()
                    .ForMember(dest => dest.StorageLocationId, opt => opt.Ignore());

                config.CreateMap<Product, ProductDto>();
                config.CreateMap<ProductDto, Product>()
                    .ForMember(dest => dest.ProductId, opt => opt.Ignore());

                config.CreateMap<Bill, BillDto>();
                config.CreateMap<BillDto, Bill>()
                    .ForMember(dest => dest.BillId, opt => opt.Ignore());

                config.CreateMap<Upd, UpdDto>();
                config.CreateMap<UpdDto, Upd>()
                    .ForMember(dest => dest.UpdId, opt => opt.Ignore());

                config.CreateMap<Provider, ProviderDto>();
                config.CreateMap<ProviderDto, Provider>()
                    .ForMember(dest => dest.ProviderId, opt => opt.Ignore());
            });
        }
    }
}

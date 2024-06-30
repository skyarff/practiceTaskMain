using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockService;
using StockService.Repository.BillRep;
using StockService.Repository.CompanyRep;
using StockService.Repository.CookieRep;
using StockService.Repository.EmployeeRep;
using StockService.Repository.ProductCategoryRep;
using StockService.Repository.ProductRep;
using StockService.Repository.ProviderRep;
using StockService.Repository.StockRep;
using StockService.Repository.StorageLocationRep;
using StockService.Repository.UpdRep;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


IMapper mapper = MappingConfig.RegistrMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IStockService, StockService.Repository.StockRep.StockService>();
builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();
builder.Services.AddScoped<IStorageLocationService, StorageLocationService>();
builder.Services.AddScoped<IProviderService, ProviderService>();
builder.Services.AddScoped<IBillService, BillService>();
builder.Services.AddScoped<IUpdService, UpdService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ICookieService, CookieService>();


builder.Services.AddDbContext<StockContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("PgSQL")));


AppSettings.PathSettings.Initialize(builder.Configuration);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();




namespace AppSettings
{
    public static class PathSettings
    {
        public static Dictionary<string, string> ImagePaths { get; private set; }

        public static void Initialize(IConfiguration configuration)
        {
            ImagePaths = configuration.GetSection("ImagePaths")
                                      .Get<Dictionary<string, string>>();
        }
    }
}




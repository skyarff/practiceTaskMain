using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StockService;
using StockService.Repository.BillRep;
using StockService.Repository.CompanyRep;
using StockService.Repository.CookieRep;
using StockService.Repository.EmployeeRep;
using StockService.Repository.JwtRep;
using StockService.Repository.ProductCategoryRep;
using StockService.Repository.ProductRep;
using StockService.Repository.ProviderRep;
using StockService.Repository.StockRep;
using StockService.Repository.StorageLocationRep;
using StockService.Repository.UpdRep;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            //ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            RoleClaimType = ClaimTypes.Role,

        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"OnAuthenticationFailed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine($"OnTokenValidated: {context.SecurityToken}");
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                Console.WriteLine($"OnChallenge: {context.Error}, {context.ErrorDescription}");
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization(options =>
{
    //options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
});


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
builder.Services.AddScoped<ITokenService, TokenService>();



builder.Services.AddDbContext<StockContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("PgSQL")));


AppSettings.PathSettings.Initialize(builder.Configuration);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
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




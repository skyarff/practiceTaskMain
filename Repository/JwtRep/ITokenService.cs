using StockService.Models;

namespace StockService.Repository.JwtRep
{
    public interface ITokenService
    {
        string GenerateToken(Employee employee);
    }
}

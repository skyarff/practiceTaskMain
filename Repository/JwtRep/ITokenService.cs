using StockService.Models;

namespace StockService.Repository.JwtRep
{
    public interface ITokenService
    {
        TokenPair GenerateTokenPair(Employee employee);
        string GenerateRefreshToken();
    }
}

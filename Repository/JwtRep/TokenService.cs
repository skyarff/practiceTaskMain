using Microsoft.IdentityModel.Tokens;
using StockService.Models;
using StockService.Repository.JwtRep;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public TokenPair GenerateTokenPair(Employee employee)
    {
        if (!(employee.Role == "StockLevelWorker" && employee?.StockId != null
            || employee.Role == "CompanyLevelWorker" && employee?.CompanyId != null
            || employee.Role == "Admin"))
            throw new Exception("Некорректные данные запроса");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, employee.EmployeeId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, employee.Role),

            new Claim("CompanyId", employee?.CompanyId != null ? employee.CompanyId.ToString() : "-1"),
            new Claim("StockId", employee?.StockId != null ? employee.StockId.ToString() : "-1"),
        };


        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            //audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        var refreshToken = GenerateRefreshToken();

        return new TokenPair() { AccessToken = accessToken, RefreshToken = refreshToken }; 
    }

    public bool IsValidRefreshToken(string refreshToken, Employee employee)
    {
        if (string.IsNullOrEmpty(refreshToken) || employee == null)
            return false;

        if (refreshToken != employee.RefreshToken)
            return false;

        //if (employee.RefreshTokenExpiryTime <= DateTime.UtcNow)
        //    throw new SecurityTokenException("Incorrect data.");

        return true;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
using Microsoft.IdentityModel.Tokens;
using StockService.Models;
using StockService.Models.dto;
using StockService.Repository.JwtRep;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(Employee employee)
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

        //if (employee?.CompanyId != null)
        //    claims.Add(new Claim("CompanyId", employee.CompanyId.ToString()));
        //if (employee?.StockId != null)
        //    claims.Add(new Claim("StockId", employee.StockId.ToString()));


        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            //audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public bool ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = false,
                //ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return true;
        }
        catch
        {
            return false;
        }
    }
}
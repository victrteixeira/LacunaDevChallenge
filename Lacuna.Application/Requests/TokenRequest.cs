using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Lacuna.Application.Interfaces;
using Lacuna.Application.Responses;
using Lacuna.Domain.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Lacuna.Application.Requests;

public class TokenRequest : ITokenRequest
{
    private readonly IConfiguration _configuration;

    public TokenRequest(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<TokenResponse> GenerateToken(User user)
    {
        return await Task.Run(() =>
        {
            var secureKey = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var securityKey = new SymmetricSecurityKey(secureKey);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Username)
                }),
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["Jwt:ValidForMinutes"])),
                Audience = audience,
                Issuer = issuer,
                SigningCredentials = credentials
            };

            var previewToken = jwtTokenHandler.CreateToken(tokenDescriptor);
            var token = jwtTokenHandler.WriteToken(previewToken);
            return new TokenResponse
            {
                AccessToken = token,
                Code = "Success"
            };
        });
    }
}
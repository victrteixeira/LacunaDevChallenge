using System.Text;
using Lacuna.Application.Interfaces;
using Lacuna.Application.Requests;
using Lacuna.Application.Services;
using Lacuna.Application.Utils;
using Lacuna.Domain.Interfaces;
using Lacuna.Infrastructure.Context;
using Lacuna.Infrastructure.Repository;
using Lacuna.Infrastructure.SeedDatabase;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Lacuna.IoC;

public static class Container
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISeedUserInitial, SeedUserInitial>();
        
        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuthentication, Authentication>();
        services.AddScoped<ITokenRequest, TokenRequest>();
        services.AddScoped<IPasswordValidation, PasswordValidation>();
        
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(opts =>
        {
            opts.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
            };
        });
        
        return services;
    }
}
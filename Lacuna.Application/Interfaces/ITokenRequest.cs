using Lacuna.Application.Responses;
using Lacuna.Domain.Users;

namespace Lacuna.Application.Interfaces;

public interface ITokenRequest
{
    Task<TokenResponse> GenerateToken(User user);
}
using Lacuna.Application.DTO;
using Lacuna.Application.Responses;

namespace Lacuna.Application.Interfaces;

public interface IAuthentication
{
    Task<CreateUserResponse> Register(CreateUserDto userDto);
    Task<TokenResponse> Login(LoginUserDto userDto);
}
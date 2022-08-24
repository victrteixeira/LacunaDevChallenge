using System.Security.Authentication;
using Lacuna.Application.DTO;
using Lacuna.Application.Interfaces;
using Lacuna.Application.Responses;
using Lacuna.Application.Utils;
using Lacuna.Domain.Interfaces;
using Lacuna.Domain.Users;

namespace Lacuna.Application.Services;

public class Authentication : IAuthentication
{
    private readonly IUserRepository _repository;
    private readonly ITokenRequest _token;

    public Authentication(IUserRepository repository, ITokenRequest token)
    {
        _repository = repository;
        _token = token;
    }
    
    public async Task<CreateUserResponse> Register(CreateUserDto userDto)
    {
        var emailExist = await _repository.GetEmailAsync(userDto.Email);
        if (emailExist != null)
            throw new AuthenticationException("Email already registered.");

        User user = new(userDto.Username, userDto.Email, userDto.Password);
        var userCreated = await _repository.CreateUserAsync(user);
        if (userCreated == null)
            throw new NullReferenceException("Something got wrong.");

        return new CreateUserResponse { Code = "Success", Message = "User created successfully." };
    }

    public async Task<TokenResponse> Login(LoginUserDto userDto)
    {
        var user = await _repository.GetUsernameAsync(userDto.Username);
        if (user == null)
            throw new NullReferenceException();

        var pwdValid = PasswordValidation.IsValid(userDto, user.HashedPassword);
        if (!pwdValid)
            throw new AuthenticationException();

        return await _token.GenerateToken(user);
    }
}
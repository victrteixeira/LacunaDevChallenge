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
    private readonly IPasswordValidation _pwdValidation;

    public Authentication(IUserRepository repository, ITokenRequest token, IPasswordValidation pwdValidation)
    {
        _repository = repository;
        _token = token;
        _pwdValidation = pwdValidation;
    }
    
    public async Task<CreateUserResponse> Register(CreateUserDto userDto)
    {
        var emailExist = await _repository.GetEmailAsync(userDto.Email);
        if (emailExist != null)
            throw new AuthenticationException("Email already registered.");

        User user = new(userDto.Username, userDto.Email, userDto.Password);
        await _repository.CreateUserAsync(user);

        return new CreateUserResponse { Code = "Success", Message = "User created successfully." };
    }

    public async Task<TokenResponse> Login(LoginUserDto userDto)
    {
        var user = await _repository.GetUsernameAsync(userDto.Username);
        if (user == null)
            throw new NullReferenceException("User not found.");

        var pwdValid = _pwdValidation.IsValid(userDto, user.HashedPassword);
        if (!pwdValid)
            throw new AuthenticationException();

        return await _token.GenerateToken(user);
    }
}
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
        {
            return new CreateUserResponse
            {
                Code = "Error",
                Message = "Email already exist."
            };
        }

        User user = new(userDto.Username, userDto.Email, userDto.Password);
        var userCreated = await _repository.CreateUserAsync(user);
        if (userCreated == null)
        {
            return new CreateUserResponse
            {
                Code = "False",
                Message = "Something went wrong."
            };    
        }

        return new CreateUserResponse { Code = "Success", Message = "User created successfully." };
    }

    public async Task<TokenResponse> Login(LoginUserDto userDto)
    {
        var user = await _repository.GetUsernameAsync(userDto.Username);
        if (user == null)
        {
            return new TokenResponse
            {
                AccessToken = null,
                Code = "Error",
                Message = "User doesn't exist."
            };
        }

        var pwdValid = PasswordValidation.IsValid(userDto, user.HashedPassword);
        if (!pwdValid)
            return new TokenResponse { AccessToken = null, Code = "Error", Message = "Login or password incorrect." };

        return await _token.GenerateToken(user);
    }
}
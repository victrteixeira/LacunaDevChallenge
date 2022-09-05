using Bogus;
using Bogus.DataSets;
using Lacuna.Application.DTO;
using Lacuna.Domain.Users;

namespace Lacuna.ApplicationTests.Fixtures;

public static class RepoFixture
{
    public static CreateUserDto ValidCreateUserDto()
    {
        var userDto = new CreateUserDto();
        userDto.Email = new Internet().Email();
        userDto.Username = "sampleusername";
        userDto.Password = new Internet().Password(12);
        userDto.ConfirmPassword = userDto.Password;

        return userDto;
    }

    public static List<CreateUserDto> ListOfValidCreateUserDto()
    {
        List<CreateUserDto> list = new();
        for (int i = 0; i < 10; i++)
        {
            var userDto = new CreateUserDto();
            userDto.Email = new Internet().Email();
            userDto.Username = "sampleusername";
            userDto.Password = new Internet().Password(12);
            userDto.ConfirmPassword = userDto.Password;
            
            list.Add(userDto);
        }

        return list;
    }

    public static User ValidUser()
    {
        var users = new Faker<User>()
            .CustomInstantiator(_ => new User
            (
                "sampleusername",
                new Internet().Email(),
                new Internet().Password(12)
            )).Generate();

        return users;
    }
    
    public static List<User> ListOfValidUsers()
    {
        var users = new Faker<User>()
            .CustomInstantiator(_ => new User
            (
                "sampleusername",
                new Internet().Email(),
                new Internet().Password(12)
            )).Generate(10);

        return users;
    }

    public static LoginUserDto ValidLoginDto()
    {
        LoginUserDto loginDto = new();
        loginDto.Username = "sampleusername";
        loginDto.Password = new Internet().Password();

        return loginDto;
    }
    
    public static List<LoginUserDto> ListOfValidLoginDto()
    {
        List<LoginUserDto> list = new();
        for (int i = 0; i < 10; i++)
        {
            LoginUserDto loginDto = new();
            loginDto.Username = "sampleusername";
            loginDto.Password = new Internet().Password();
            
            list.Add(loginDto);
        }

        return list;
    }
}
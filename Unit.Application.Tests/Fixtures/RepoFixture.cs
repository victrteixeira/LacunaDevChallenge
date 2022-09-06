using Bogus;
using Bogus.DataSets;
using Lacuna.Application.DTO;
using Lacuna.Domain.Users;

namespace Unit.Application.Tests.Fixtures;

public static class RepoFixture
{
    public static CreateUserDto ValidCreateUserDto()
    {
        var userDto = new CreateUserDto();
        userDto.Email = new Internet().Email();
        userDto.Username = new Randomizer().Replace(new Randomizer().ClampString(new Name().FirstName(), 7, 31));
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
            userDto.Username = new Randomizer().Replace(new Randomizer().ClampString(new Name().FirstName(), 7, 31));
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
                new Randomizer().Replace(new Randomizer().ClampString(new Name().FirstName(), 7, 31)),
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
                new Randomizer().Replace(new Randomizer().ClampString(new Name().FirstName(), 7, 31)),
                new Internet().Email(),
                new Internet().Password(12)
            )).Generate(10);

        return users;
    }

    public static LoginUserDto ValidLoginDto()
    {
        LoginUserDto loginDto = new();
        loginDto.Username = new Randomizer().Replace(new Randomizer().ClampString(new Name().FirstName(), 7, 31));
        loginDto.Password = new Internet().Password();

        return loginDto;
    }
    
    public static List<LoginUserDto> ListOfValidLoginDto()
    {
        List<LoginUserDto> list = new();
        for (int i = 0; i < 10; i++)
        {
            LoginUserDto loginDto = new();
            loginDto.Username = new Randomizer().Replace(new Randomizer().ClampString(new Name().FirstName(), 7, 31));
            loginDto.Password = new Internet().Password();
            
            list.Add(loginDto);
        }

        return list;
    }
}
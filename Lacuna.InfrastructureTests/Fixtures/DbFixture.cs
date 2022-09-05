using Bogus;
using Bogus.DataSets;
using Lacuna.Domain.Users;

namespace Lacuna.InfrastructureTests.Fixtures;

public static class DbFixture
{
    public static List<User> UserPopulate()
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
    
    public static User DbValidEntity()
    {
        return new Faker<User>()
            .CustomInstantiator(_ => new User
            (
                "sampleusername",
                new Internet().Email(),
                new Internet().Password(12)
            )).Generate();
    }
    
    public static User DbNonValidUser()
    {
        return new User(new Randomizer().ClampString(new Name().FirstName(), 5, 31), "", new Internet().Password(12));
    }
}
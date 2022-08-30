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
                new Name().FirstName(),
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
                new Name().FirstName(),
                new Internet().Email(),
                new Internet().Password(12)
            )).Generate();
    }
    
    public static User DbNonValidUser()
    {
        return new User(new Internet().Email(), "", new Internet().Password(12));
    }
}
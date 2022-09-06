using Bogus;
using Bogus.DataSets;
using Lacuna.Domain.Users;
using Lacuna.Infrastructure.Context;

namespace Integration.Application.Tests.Environment;

public static class Utilities
{
    public static void InitializeDatabase(LacunaContext db)
    {
        db.LacunaUsers.AddRange(SeedDatabase());
        db.SaveChanges();
    }

    public static List<User> SeedDatabase()
    {
        List<User> userList = new()
        {
            new User(new Randomizer().Replace(new Randomizer().ClampString(new Name().FirstName(), 7, 31)),
                new Internet().Email(), new Internet().Password(12)),
            
            new User(new Randomizer().Replace(new Randomizer().ClampString(new Name().FirstName(), 7, 31)),
                new Internet().Email(), new Internet().Password(12)),
            
            new User(new Randomizer().Replace(new Randomizer().ClampString(new Name().FirstName(), 7, 31)),
                new Internet().Email(), new Internet().Password(12)),
            
            new User(new Randomizer().Replace(new Randomizer().ClampString(new Name().FirstName(), 7, 31)),
                new Internet().Email(), new Internet().Password(12)),
            
            new User("sampleuser", "sampleuser@email.com", "samplePassw0rd@123")
        };

        return userList;
    }
}
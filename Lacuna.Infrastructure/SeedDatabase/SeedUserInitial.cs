using Lacuna.Domain.Interfaces;
using Lacuna.Domain.Users;

namespace Lacuna.Infrastructure.SeedDatabase;

public class SeedUserInitial : ISeedUserInitial
{
    private readonly IUserRepository _repository;

    public SeedUserInitial(IUserRepository repository)
    {
        _repository = repository;
    }

    public void SeedUser()
    {
        if (_repository.GetEmailAsync("firstEmail@sample.com").Result == null)
        {
            var newUser = new User("firstUsername", "firstEmail@sample.com", "samplepwd@123");
            _repository.CreateUserAsync(newUser);
        }
        
        if (_repository.GetEmailAsync("secondEmail@sample.com").Result == null)
        {
            var newUser = new User("secondUsername", "secondEmail@sample.com", "samplepwd@123");
            _repository.CreateUserAsync(newUser);
        }
        
        if (_repository.GetEmailAsync("thirdEmail@sample.com").Result == null)
        {
            var newUser = new User("thirdUsername", "thirdEmail@sample.com", "samplepwd@123");
            _repository.CreateUserAsync(newUser);
        }
    }
}
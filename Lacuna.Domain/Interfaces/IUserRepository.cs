using Lacuna.Domain.Users;

namespace Lacuna.Domain.Interfaces;

public interface IUserRepository
{
    Task<bool> CreateUserAsync(User user);
    Task DeleteUserAsync(User user);
    Task<User> GetUserAsync(string email);
}
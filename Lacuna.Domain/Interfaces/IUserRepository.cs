using Lacuna.Domain.Users;

namespace Lacuna.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> CreateUserAsync(User user);
    Task DeleteUserAsync(User user);
    Task<User?> GetEmailAsync(string email);
    Task<User?> GetUsernameAsync(string username);
}
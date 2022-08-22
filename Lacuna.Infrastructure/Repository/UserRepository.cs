using Lacuna.Domain.Users;
using Lacuna.Domain.Interfaces;
using Lacuna.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Lacuna.Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    private readonly LacunaContext _context;
    public UserRepository(LacunaContext context)
    {
        _context = context;
    }
    public async Task<bool> CreateUserAsync(User user)
    {
        _context.LacunaUsers.Add(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task DeleteUserAsync(User user)
    {
        _context.LacunaUsers.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User> GetUserAsync(string email)
    {
        return await _context.LacunaUsers
            .AsNoTracking()
            .Where(x => x.Email.ToLower().Contains(email.ToLower()))
            .FirstOrDefaultAsync();
    }
}
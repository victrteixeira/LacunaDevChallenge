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
    public async Task<User?> CreateUserAsync(User user)
    {
        _context.LacunaUsers.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task DeleteUserAsync(User user)
    {
        _context.LacunaUsers.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetEmailAsync(string email)
    {
        var query = await _context.LacunaUsers
            .AsNoTracking()
            .Where(x => x.Email.ToLower().Contains(email.ToLower()))
            .FirstOrDefaultAsync();

        if (query == default)
            return null;
        return query;
    }

    public async Task<User?> GetUsernameAsync(string username)
    {
       var query = await _context.LacunaUsers
            .AsNoTracking()
            .Where(x => x.Username.ToLower()
                .Contains(username.ToLower()))
            .FirstOrDefaultAsync();

       if (query == default)
           return null;
       return query;
    }
}
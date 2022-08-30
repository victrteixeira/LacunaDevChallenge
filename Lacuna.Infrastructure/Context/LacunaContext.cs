using Lacuna.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Lacuna.Infrastructure.Context;

public class LacunaContext : DbContext
{
    public LacunaContext(DbContextOptions<LacunaContext> options) : base(options)
    {
    }

    public LacunaContext()
    {
    }
    
    public virtual DbSet<User> LacunaUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LacunaContext).Assembly);
    }
}
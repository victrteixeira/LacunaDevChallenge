using Lacuna.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lacuna.Infrastructure.Map;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Username)
            .IsRequired()
            .HasMaxLength(32)
            .HasColumnType("VARCHAR(32)")
            .HasColumnName("UserName");

        builder.Property(x => x.Email)
            .IsRequired()
            .HasColumnName("E-Mail")
            .HasColumnType("VARCHAR(100)");

        builder.Property(x => x.Password)
            .HasColumnName("HashPassword")
            .HasColumnType("VARCHAR(64)");
    }
}
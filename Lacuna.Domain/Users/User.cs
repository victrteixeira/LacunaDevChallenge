using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Lacuna.Domain.Validation;

namespace Lacuna.Domain.Users;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; private set; }
    public string Email { get; private set; }
    public string HashedPassword { get; private set; }

    protected User()
    {
    }
    public User(string username, string email, string password)
    {
        ValidateDomain(username, email, password);
    }

    private void ValidateDomain(string username, string email, string password)
    {
        DomainExceptionValidation.When(string.IsNullOrEmpty(username), "Invalid username.");
        DomainExceptionValidation.When(!UsernameIsValid(username), "Invalid username.");
        DomainExceptionValidation.When(username.Length < 4 || username.Length > 32, "Username invalid, the minimum are 3 characters and maximum are 32 characters.");
        DomainExceptionValidation.When(!EmailIsValid(email), "Invalid email.");
        DomainExceptionValidation.When(password.Length < 8, "Password must has 8 chars at minimum.");

        Id = Guid.NewGuid();
        Username = username;
        Email = email;
        HashedPassword = HashPassword(password);
    }

    private static bool UsernameIsValid(string str)
    {
        return Regex.IsMatch(str, @"^[0-9a-zA-Z]+$");
    }

    private static bool EmailIsValid(string str)
    {
        return Regex.IsMatch(str, @"[a-z A-z 0-9_\-]+[@]+[a-z]+[\.][a-z]{3,4}$");
    }

    public static string HashPassword(string password)
    {
        var sha256 = HashAlgorithmName.SHA256;
        byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
        byte[] hash = new Rfc2898DeriveBytes(password, salt, 100000, sha256).GetBytes(20);

        byte[] hashedBytes = new byte[36];
        Array.Copy(salt, 0, hashedBytes,0,16);
        Array.Copy(hash,0,hashedBytes,16,20);
        return Convert.ToBase64String(hashedBytes);
    }
}
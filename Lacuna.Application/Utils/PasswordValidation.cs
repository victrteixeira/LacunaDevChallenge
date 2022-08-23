using System.Security.Cryptography;
using Lacuna.Application.DTO;
using Lacuna.Application.Interfaces;

namespace Lacuna.Application.Utils;

public static class PasswordValidation
{
    public static bool IsValid(LoginUserDto userDto, string hashedPwd)
    {
        byte[] salt = new byte[16];
        byte[] hashBytes = Convert.FromBase64String(hashedPwd);
        Array.Copy(hashBytes, 0, salt, 0, 16);
        var pbkdf2 = new Rfc2898DeriveBytes(userDto.Password, salt, 100000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(20);
        for (int i = 0; i < 20; i++)
            if (hashBytes[i + 16] != hash[i])
                return false;

        return true;
    }
}
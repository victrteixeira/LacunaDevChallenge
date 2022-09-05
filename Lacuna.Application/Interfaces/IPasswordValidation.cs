using Lacuna.Application.DTO;

namespace Lacuna.Application.Interfaces;

public interface IPasswordValidation
{
    bool IsValid(LoginUserDto userDto, string hashedPwd);
}
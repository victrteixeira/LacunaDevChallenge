using System.ComponentModel.DataAnnotations;

namespace Lacuna.Application.DTO;

public class LoginUserDto
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}
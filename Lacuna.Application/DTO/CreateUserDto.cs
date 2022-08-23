using System.ComponentModel.DataAnnotations;

namespace Lacuna.Application.DTO;

public class CreateUserDto
{
    [Required(ErrorMessage = "Username cannot be null or empty.")]
    [RegularExpression(@"^[0-9a-zA-Z]+$", ErrorMessage = "Username invalid.")]
    public string Username { get; set; }
    
    [Required(ErrorMessage = "Email cannot be null or empty")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Email invalid.")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Password cannot be null or empty.")]
    [MinLength(8, ErrorMessage = "Password must have 8 characters at minimum.")]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "A confirm password must be provided.")]
    [Compare("Password", ErrorMessage = "Password don't match.")]
    public string ConfirmPassword { get; set; }
}
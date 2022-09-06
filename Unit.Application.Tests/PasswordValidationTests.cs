namespace Unit.Application.Tests;

public class PasswordValidationTests
{
    private readonly IPasswordValidation _sut;
    
    private User _user;
    private LoginUserDto _loginAttempt;

    public PasswordValidationTests()
    {
        _user = new User("sampleusername", new Internet().Email(), "sAmplEPassword1234");
        _loginAttempt = new LoginUserDto();

        _sut = new PasswordValidation();
    }
    
    [Fact(DisplayName = "Verify If Login Password and User Password Match")]
    [Trait("Validation", "Utils")]
    public void PasswordValidation_VerifyIfPasswordsMatch_ShouldReturnTrue()
    {
        // Arrange
        _loginAttempt.Username = _user.Username;
        _loginAttempt.Password = "sAmplEPassword1234";
        
        // Act
        var res = _sut.IsValid(_loginAttempt, _user.HashedPassword);
        
        // Assert
        res.Should().BeTrue();
    }

    [Fact(DisplayName = "Verify If Login Password and User Password Doesn't Match")]
    [Trait("Validation", "Utils")]
    public void PasswordValidation_VerifyIfPasswordsMatch_ShouldReturnFalse()
    {
        // Arrange
        _loginAttempt.Username = _user.HashedPassword;
        _loginAttempt.Password = "sAmplEPassword123";
        
        // Act
        var res = _sut.IsValid(_loginAttempt, _user.HashedPassword);
        
        // Assert
        res.Should().BeFalse();
    }
}